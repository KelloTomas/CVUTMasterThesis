using ServerApp.Data;
using ServerApp.SubApps;
using ServerApp.SubApps.Shared.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
	class Server
	{
		List<ISubApp> SubApps;
		IStateBase currState;
		public void Start()
		{
			foreach(ISubApp subApp in SubApps)
			{
				RunSubApp(subApp);
			}
		}
		private void RunSubApp(ISubApp subApp)
		{
			currState = subApp.Init();
			subApp.SubscribeToActions(ProcessAction);
			// ToDo subscribe for event subApp.
			while(true)
			{
				IStateBase newState = currState.ProcessTimerElapsed();
				CheckNewState(newState);
			}
		}
		private void ProcessAction(IAction action)
		{
			IStateBase newState;
			switch(action)
			{
				case ButtonClickAction b:
					newState = currState.ProcessButtonClickAction(b);
					break;
				case CardReadAction c:
					newState = currState.ProcessCardReadAction(c);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			CheckNewState(newState);
		}
		private void CheckNewState(IStateBase newState)
		{
			if (newState != null)
			{
				currState.Exit();
				newState.Enter();
				currState = newState;
			}
		}
	}
}
