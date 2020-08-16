﻿namespace Dash.Engine.Abstractions
{
    public interface IConsole
    {
        void Trace(string message);
        void Info(string message);
        void Error(string errorMessage);
    }
}