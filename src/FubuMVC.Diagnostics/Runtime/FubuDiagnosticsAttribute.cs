using System;
using FubuMVC.Core;

namespace FubuMVC.Diagnostics.Runtime
{
    [MoveToDiagnostics]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class FubuDiagnosticsAttribute : Attribute
    {
        private readonly string _description;

        public FubuDiagnosticsAttribute(string description)
        {
            _description = description;
        }

        public string Description
        {
            get { return _description; }
        }

        public bool ShownInIndex { get; set; }
    }
}