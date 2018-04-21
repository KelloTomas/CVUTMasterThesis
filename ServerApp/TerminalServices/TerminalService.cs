using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using ServerApp.Devices;
using ServerApp.Devices.Actions;
using ServerApp.TerminalServices.Shared.States;
using DataLayer;

namespace ServerApp.TerminalServices
{
	public class TerminalService : ITerminalService
	{
		public DatabaseLayer databaseLayer;
		private Timer t;

		public TerminalService(string appName, DatabaseLayer databaseLayer)
		{
			this.databaseLayer = databaseLayer;
			AppName = appName;
		}

		public void Start()
		{
			ActualState = GetInitState();
			t = new Timer();
			t.Elapsed += TimerElapsed;
			t.AutoReset = true;
			t.Interval = ActualState.TimeOut;
			t.Start();
			ActualState.Enter();
			CheckNewState(ActualState.ProcessTimerElapsed());
		}

		private void TimerElapsed(object sender, ElapsedEventArgs e)
		{
			Console.WriteLine($"TimeElapsed: {GetType()}");
			CheckNewState(ActualState.ProcessTimerElapsed());
		}
		public string AppName { get; private set; }

		public IStateBase ActualState { get; set; }

		public IStateBase IdleState => throw new NotImplementedException();


		public virtual IStateBase GetInitState()
		{
			throw new NotImplementedException();
		}

		IStateBase ITerminalService.GetInitState()
		{
			throw new NotImplementedException();
		}

		public virtual IEnumerable<IStoreLayoutAction> GetStoreLayoutActions()
		{
			return Enumerable.Empty<IStoreLayoutAction>();
		}

		public void ProcessAction(IAction action)
		{
			bool forceCallStateMethod = false;
			IStateBase newState = ActualState.ProcessAction(action, ref forceCallStateMethod);
			if (!CheckNewState(newState))
				if (forceCallStateMethod)
				{
					ActualState.ProcessTimerElapsed();
					t.Stop();
					t.Start();
				}
		}

		public IEnumerable<IAction> InitActions
		{
			get
			{
				foreach (IStoreLayoutAction storeLayoutAction in GetStoreLayoutActions())
				{
					yield return storeLayoutAction;
				}
			}
		}

		public bool CheckNewState(IStateBase newState)
		{
			if (newState != ActualState)
			{
				t.Stop();
				ActualState?.Exit();
				newState.Enter();
				t.Start();
				ActualState = newState;
				return true;
			}
			return false;
		}
	}
}
