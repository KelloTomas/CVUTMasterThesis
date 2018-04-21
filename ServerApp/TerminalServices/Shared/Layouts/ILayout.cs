using System;
using System.Linq;

namespace ServerApp.TerminalServices.Shared.Layouts
{
    public interface ILayout
    {
        string GetLayout();

        string Name { get; }
    }
}
