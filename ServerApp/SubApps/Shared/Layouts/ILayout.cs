using System;
using System.Linq;

namespace ServerApp.SubApps.Shared.Layouts
{
    public interface ILayout
    {
        string GetLayout();

        string Name { get; }
    }
}
