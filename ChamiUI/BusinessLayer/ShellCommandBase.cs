﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Chami.Db.Entities;
using ChamiUI.PresentationLayer.Progress;

namespace ChamiUI.BusinessLayer
{
    /// <summary>
    /// Base class that provides common protected methods for classes that implement <see cref="IShellCommand"/>.
    /// </summary>
    public abstract class ShellCommandBase : IShellCommand
    {
        /// <summary>
        /// Derived classes must implement the <see cref="Execute"/> method.
        /// </summary>
        public abstract void Execute();


        /// <summary>
        /// Derived classes must implement the <see cref="ExecuteAsync"/> method.
        /// </summary>
        /// <param name="progress">Notifies the caller of progress.</param>
        /// <param name="percentage">Percentage of overall completion</param>
        /// <param name="cancellationToken">Allows cancelling the task.</param>
        /// <returns></returns>
        public abstract Task ExecuteAsync(IProgress<CmdExecutorProgress> progress, float percentage,
            CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new <see cref="Process"/> object that can be started by an inheritor.
        /// </summary>
        /// <param name="arguments">The command line to execute by the process.</param>
        /// <returns>A <see cref="Process"/> object for use by an <see cref="CmdExecutorBase"/>.</returns>
        protected virtual Process PrepareProcess(string arguments)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", arguments)
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            Process process = new Process()
            {
                StartInfo = processStartInfo
            };
            return process;
        }
    }
}