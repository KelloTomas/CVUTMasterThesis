using DataLayer.Data;
using ServerApp.TerminalServices.Shared.Data;
using ServerApp.TerminalServices.Shared.Layouts;
using System.Collections.Generic;

namespace ServerApp.TerminalServices.Order.Layouts
{
    // obrazovka s textom pre klienta
    public class MessageLayout : LayoutTimeBase
    {

        #region constructors...
        public MessageLayout() : base()
        {
        }
        #endregion

        public IEnumerable<ModifyLayoutItem> SetText(Client client, string clientMsg)
        {
            if (client == null)
            {
                yield return new ModifyLayoutItem("ClientName", "text", $"--");
                yield return new ModifyLayoutItem("AccountBalance", "text", $" ");
            }
            else
            {
                yield return new ModifyLayoutItem("ClientName", "text", $"{client.LastName} {client.FirstName}");
                yield return new ModifyLayoutItem("AccountBalance", "text", $"{client.Balance} Kc");
            }
            yield return new ModifyLayoutItem($"contentValue", "text", clientMsg);
        }
    }
}