﻿// <file>
//     <copyright see="prj:///doc/copyright.txt">2002-2005 AlphaSierraPapa</copyright>
//     <license see="prj:///doc/license.txt">GNU General Public License</license>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision$</version>
// </file>

using System;
using System.Collections;
using System.Runtime.InteropServices;

using Debugger;
using Debugger.Interop.CorDebug;
using Debugger.Interop.MetaData;

namespace Debugger
{
	/// <summary>
	/// This class holds information about function evaluation.
	/// </summary>
	public class Eval 
	{
		NDebugger debugger;
		
		ICorDebugEval     corEval;
		ICorDebugFunction corFunction;
		ICorDebugValue[]  args;
		
		bool              evaluating = false;
		bool              completed = false;
		bool              successful = false;
		Value             result;
		
		public event EventHandler<EvalEventArgs> EvalStarted;
		public event EventHandler<EvalEventArgs> EvalComplete;
		
		public NDebugger Debugger {
			get {
				return debugger;
			}
		}
		
		/// <summary>
		/// True if the evaluation has been completed.
		/// </summary>
		public bool Completed {
			get {
				return completed;
			}
		}
		
		/// <summary>
		/// True if the eval is being evaluated at the moment.
		/// </summary>
		public bool Evaluating {
			get {
				return evaluating;
			}
			set {
				evaluating = value;
			}
		}
		
		/// <summary>
		/// True if the evaluation was successful, false if it thown an exception (which is presented as the result)
		/// </summary>
		public bool Successful {
			get {
				return successful;
			}
			internal set {
				successful = value;
			}
		}
		
		/// <summary>
		/// The result of the evaluation if the evaluation is complete and has returned a value. Null otherwise.
		/// </summary>
		public Value Result {
			get {
				if (completed) {
					if (successful) {
						return result;
					} else {
						ObjectValue exception = (ObjectValue)result;
						while (exception.Type != "System.Exception") {
							exception = exception.BaseClass;
						}
						return new UnavailableValue(debugger, result.Type + ": " + exception["_message"].Value.AsString);
					}
				} else {
					return null;
				}
			}
		}
		
		internal ICorDebugEval CorEval {
			get {
				return corEval;
			}
		}
		
		internal Eval(NDebugger debugger, ICorDebugFunction corFunction, ICorDebugValue[] args)
		{
			this.debugger = debugger;
			this.corFunction = corFunction;
			this.args = args;
		}
		
		/// <returns>True is setup was successful</returns>
		internal bool SetupEvaluation(Thread targetThread)
		{
			if (!debugger.ManagedCallback.HandlingCallback) debugger.AssertPaused();
			
			// TODO: What if this thread is not suitable?
			targetThread.CorThread.CreateEval(out corEval);
			
			corEval.CallFunction(corFunction, (uint)args.Length, args);
			
			evaluating = true;
			
			OnEvalStarted(new EvalEventArgs(this));
			
			return true;
		}
		
		protected virtual void OnEvalStarted(EvalEventArgs e)
		{
			if (EvalStarted != null) {
				EvalStarted(this, e);
			}
		}
		
		protected internal virtual void OnEvalComplete(EvalEventArgs e) 
		{
			evaluating = false;
			completed = true;
			
			ICorDebugValue corValue;
			corEval.GetResult(out corValue);
			result = Value.CreateValue(debugger, corValue);
				
			if (EvalComplete != null) {
				EvalComplete(this, e);
			}
		}
	}
}
