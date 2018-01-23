using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using ServerApp.Devices;
using ServerApp.Devices.Actions;
using ServerApp.SubApps.Shared.States;

namespace ServerApp.SubApps
{
	public class SubApp : ISubApp
	{
		public CVUTdbEntities db = new CVUTdbEntities();
		private Timer t;

		public SubApp()
		{
		}

		public void Start()
		{
			ActualState = GetInitState();
			t = new Timer();
			t.Elapsed += timerElapsed;
			t.AutoReset = true;
			t.Interval = ActualState.TimeOut;
			t.Start();
			CheckNewState(ActualState.ProcessTimerElapsed());
		}

		private void timerElapsed(object sender, ElapsedEventArgs e)
		{
			Console.WriteLine($"TimeElapsed: {GetType()}");
			CheckNewState(ActualState.ProcessTimerElapsed());
		}

		public bool Terminated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public IStateBase ActualState { get; set; }

		public IStateBase IdleState => throw new NotImplementedException();


		public virtual IStateBase GetInitState()
		{
			throw new NotImplementedException();
		}

		IStateBase ISubApp.GetInitState()
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

				// pote az cele obrazovky, ktere jiz mohou vyuzivat ulozene fragmenty
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
				ActualState?.Exit();
				newState.Enter();
				ActualState = newState;
				return true;
			}
			return false;
		}
	}
}
