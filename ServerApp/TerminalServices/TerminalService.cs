using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using ServerApp.Devices;
using ServerApp.Devices.Actions;
using ServerApp.TerminalServices.Shared.States;
using DataLayer;
using DataLayer.Data;

namespace ServerApp.TerminalServices
{
	// vseobecna obsluha terminalu
	public class TerminalService : ITerminalService
	{
		public DatabaseLayer databaseLayer;
		private Timer _t;

		public TerminalService(MyApplication app, DatabaseLayer databaseLayer)
		{
			this.databaseLayer = databaseLayer;
			App = app;
		}

		public void Start()
		{
			ActualState = GetInitState();
			_t = new Timer();
			_t.Elapsed += TimerElapsed;
			_t.AutoReset = true;
			_t.Interval = ActualState.TimeOut;
			_t.Start();
			ActualState.Enter();
			CheckNewState(ActualState.ProcessTimerElapsed());
		}

		private void TimerElapsed(object sender, ElapsedEventArgs e)
		{
			Console.WriteLine($"TimeElapsed: {GetType()}");
			CheckNewState(ActualState.ProcessTimerElapsed());
		}
		public MyApplication App { get; private set; }

		public IStateBase ActualState { get; set; }

		public IStateBase IdleState => throw new NotImplementedException();


		public virtual IStateBase GetInitState()
		{
			// implementation in derivated class
			throw new NotImplementedException();
		}

		IStateBase ITerminalService.GetInitState()
		{
			// implementation in derivated class
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
					_t.Stop();
					_t.Start();
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
				_t.Stop();
				ActualState?.Exit();
				newState.Enter();
				_t.Start();
				ActualState = newState;
				return true;
			}
			return false;
		}
	}
}
