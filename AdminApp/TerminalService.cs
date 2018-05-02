using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdminApp
{
    public enum SubApp
    {
        NastavIP,
        NastavHostName,
        ZistiVerziuApp,
        ZistiTeplotu,
        StiahniLogy,
        AktualizujApp,
        Putty,
        VNC,
        WinSCP,
        FileZilla,
    }
    class TerminalService
    {
        #region private fields...
        protected ConnectionInfo connInfo;
        private const string OLD_LOG_FILE_PATTERN = "QTTcpServer-*.log";
        private string _ipAddress;

        public TerminalService(string ipAddress)
        {
            _ipAddress = ipAddress;
            connInfo = new ConnectionInfo(ipAddress, "pi", new PasswordAuthenticationMethod("pi", "D1amant"))
            {
                Timeout = new TimeSpan(0, 0, 10)
            };
        }
        private string hostsFile(string hostName)
        {
            return $@"127.0.0.1       localhost\n" +
                    $@"::1             localhost ip6-localhost ip6-loopback\n" +
                    $@"ff02::1         ip6-allnodes\n" +
                    $@"ff02::2         ip6-allrouters\n\n" +
                    $@"127.0.1.1       {hostName}\n";
        }
        private string dhcpFile()
        {
            return @"hostname\n" +
                    @"clientid\n" +
                    @"persistent\n" +
                    @"option rapid_commit\n" +
                    @"option domain_name_servers, domain_name, domain_search, host_name\n" +
                    @"option classless_static_routes\n" +
                    @"option ntp_servers\n" +
                    @"require dhcp_server_identifier\n" +
                    @"slaac private\n" +
                    @"nohook lookup-hostname\n";
        }

        private string IpAddressFile(string ip, string GW, string DNS)
        {
            string output =
                    $@"hostname\n" +
                    $@"clientid\n" +
                    $@"persistent\n" +
                    $@"option rapid_commit\n" +
                    $@"slaac private\n" +
                    $@"interface eth0\n" +
                    $@"static ip_address={ip}\n";
            if (!string.IsNullOrWhiteSpace(GW))
                output += $"static routers={GW}\n";
            if (!string.IsNullOrWhiteSpace(DNS))
                output += $"static domain_name_servers={DNS}\n";
            return output;
        }
        #endregion


        public int UpdateApp(string localFileName)
        {
            string fileName = "QTTcpServer";
            RemoveOldApp(fileName);
            UploadNewApp(fileName, localFileName);
            SetFileExecutable(fileName);
            RestartDevice();
            return 0;
        }
        private void RemoveOldApp(string fileName)
        {
            Console.WriteLine($"   Removing old file {fileName}... ");
            ExecuteCommand($"rm {fileName}");
        }

        private void UploadNewApp(string remoteFileName, string localFileName)
        {
            UploadFile(localFileName, remoteFileName);
        }

        private void SetFileExecutable(string fileName)
        {
            Console.WriteLine($"   Making new file {fileName} executable...");
            SetFileMode("777", fileName);
        }

        public void Start(SubApp app, string folder)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            switch (app)
            {
                case SubApp.VNC:
                    int port = StartVNCServerOnRPi();
                    startInfo.FileName = Path.Combine(folder, @"TightVNCViewerPortable\TightVNCViewerPortable.exe");
                    startInfo.Arguments = $"-host=\"{_ipAddress}\" -port=\"{port}\" -password=\"D1amant\"";
                    break;
                case SubApp.Putty:
                    startInfo.FileName = Path.Combine(folder, @"PuTTYPortable\PuTTYPortable.exe");
                    startInfo.Arguments = $@"pi@{_ipAddress} -pw D1amant";
                    break;
                case SubApp.WinSCP:
                    startInfo.FileName = Path.Combine(folder, @"WinSCPPortable\WinSCPPortable.exe");
                    startInfo.Arguments = $@"sftp://pi:D1amant@{_ipAddress}/";
                    break;
                case SubApp.FileZilla:
                    startInfo.FileName = Path.Combine(folder, @"FileZillaPortable\FileZillaPortable.exe");
                    startInfo.Arguments = $@"sftp://pi:D1amant@{_ipAddress}/home/pi";
                    break;
                default:
                    throw new NotImplementedException("Unknown program");
            }

            // heslo nezalogovavám, nechci ho mít uloženo na disku v logu
            Console.WriteLine($"Spouštím {startInfo.FileName}");
            Process.Start(startInfo);
        }
        private int StartVNCServerOnRPi()
        {
            int port;
            string command = ("x11vnc -rfbauth .vnc/passwd -q -display :0 &");
            string tmp = ExecuteCommand(command);

            if (int.TryParse(tmp.Replace("PORT=", ""), out int p))
                port = p;
            else
                port = -1;
            return port;
        }
        public void SetIP(string ip = null, string GW = null, string DNS = null)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                ExecuteCommand($"sudo sh -c \"echo '{dhcpFile()}' > /etc/dhcpcd.conf\"");
            }
            else
            {
                ExecuteCommand($"sudo sh -c \"echo '{IpAddressFile(ip, GW, DNS)}' > /etc/dhcpcd.conf\"");
            }
            RestartDevice();
        }

        public void SetHostName(string hostName)
        {
            ExecuteCommand($"sudo sh -c \"echo '{hostName}' > /etc/hostname\"");
            ExecuteCommand($"sudo sh -c \"echo '{hostsFile(hostName)}' > /etc/hosts\"");
            RestartDevice();
        }

        public string GetVersion()
        {
            string output = ExecuteCommand("./RalloApp -v", false);
            return output;
        }

        public string GetTemperature()
        {
            string output = ExecuteCommand("/opt/vc/bin/vcgencmd measure_temp");
            output = output.Replace("temp=", "");
            return output;
        }

        #region GetLogs
        public IEnumerable<string> GetLogFileNames()
        {
            return GetLogFileFrom($"/home/pi/", OLD_LOG_FILE_PATTERN
                    ).Concat(
                        GetLogFileFrom($"/home/pi/logs/", OLD_LOG_FILE_PATTERN)
                );
        }

        public IEnumerable<string> GetLogFileFrom(string location, string pattern)
        {
            if (location.Last() != '/')
                location += '/';
            return ExecuteCommand($"ls {location}{pattern}").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public void GetLogs(string outputDirectory)
        {
            // slozku nejprve vytvorim
            Directory.CreateDirectory(outputDirectory);

            IEnumerable<string> files = GetLogFileNames();
            foreach (string file in files)
            {
                DownloadFile(file, Path.Combine(outputDirectory, $@"Logs-{file.Substring(file.IndexOf('-') + 1)}"));
                Console.WriteLine(file);
            }
        }

        public void GetLog(string fileName, string outputFileName)
        {
            DownloadFile(fileName, outputFileName);
        }
        #endregion

        #region OperationBase
        protected string ExecuteCommand(string command, bool async = true)
        {
            string tmp = string.Empty;
            using (SshClient sshClient = new SshClient(connInfo))
            {
                sshClient.Connect();
                    if (async)
                        tmp = ExecuteCommandAsync(sshClient, command);
                    else
                        tmp = ExecuteCommand(sshClient, command);

                Console.WriteLine(tmp);
                sshClient.Disconnect();
            }
            return tmp;
        }

        protected string ExecuteCommandAsync(SshClient sshClient, string command)
        {
            string tmp;
            using (SshCommand cmd = sshClient.CreateCommand(command))
            {
                cmd.BeginExecute();
                using (StreamReader reader = new StreamReader(cmd.OutputStream))
                {
                    tmp = reader.ReadToEnd();
                }
            }
            return tmp;
        }

        protected string ExecuteCommand(SshClient sshClient, string command)
        {
            string tmp;
            using (SshCommand cmd = sshClient.CreateCommand(command))
            {
                tmp = cmd.Execute();
                tmp += cmd.Error; // ToDo rozdelit na stdErr a stdOut
            }
            return tmp;
        }

        protected void DownloadFile(string remoteFileName, string localFileName)
        {
            if (string.IsNullOrWhiteSpace(remoteFileName))
                return;


            using (var client = new SftpClient(connInfo))
            {
                client.Connect();
                using (var fileStream = File.Create(localFileName))
                {
                    client.DownloadFile(remoteFileName, fileStream);
                }
                client.Disconnect();
            }
            return;
        }

        protected void UploadFile(string localFileName, string remoteFileName)
        {
            using (var client = new SftpClient(connInfo))
            {
                client.Connect();
                using (MemoryStream ms = new MemoryStream())
                {
                    using (FileStream file = new FileStream(localFileName, FileMode.Open, FileAccess.Read))
                    {
                        file.CopyTo(ms);
                        ms.Position = 0;
                        client.BufferSize = 1024 * 1024;
                        client.UploadFile(ms, remoteFileName);
                    }
                }
                client.Disconnect();
            }
        }


        protected void SetFileMode(string mode, string fileName)
        {
            Console.WriteLine($"Making new file {fileName} executable...");
            ExecuteCommand($"chmod {mode} {fileName}");
        }

        protected void RestartDevice()
        {
            Console.WriteLine($"   Restarting device ip: {_ipAddress}...");
            try
            {
                using (SshClient sshClient = new SshClient(connInfo))
                {
                    sshClient.Connect();
                    ExecuteCommand(sshClient, $"sudo reboot");
                    sshClient.Disconnect();
                }
            }
            catch (Renci.SshNet.Common.SshConnectionException e) when (e.DisconnectReason != Renci.SshNet.Messages.Transport.DisconnectReason.ConnectionLost)
            {
                Console.WriteLine(e);
            }
        }
        #endregion
    }
}
