﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using NUnit.Framework;

namespace ICSharpCode.Reports.Addin.Test.Designer
{
	[TestFixture]
	public class ReportDesignerLoaderTestFixture
	{
		IDesignerGenerator generator;
		ReportDesignerView view;
		
		[Test]
		public void CheckIfViewIsCreated()
		{
			Assert.IsNotNull(view);
		}
		
		
		[Test]
		public void CheckIfViewIsAttached()
		{
			Assert.IsNotNull(this.generator.ViewContent);
			Assert.IsInstanceOf<ReportDesignerView>(this.generator.ViewContent);
		}
		
		
		[TestFixtureSetUp]
		public void Init()
		{
			generator = new MockDesignerGenerator();
			view = new ReportDesignerView(null, new MockOpenedFile("Test.srd"));
			generator.Attach(view);
			/*
			view.DesignerCodeFileContent = GetFormCode();
			loader = new DerivedPythonDesignerLoader(generator);
	
			// Begin load.
			mockDesignerLoaderHost = new MockDesignerLoaderHost();
			mockTypeResolutionService = mockDesignerLoaderHost.TypeResolutionService;

			mockExtenderProviderService = new MockExtenderProviderService();
			mockDesignerLoaderHost.AddService(typeof(IExtenderProviderService), mockExtenderProviderService);
			
			mockEventBindingService = new MockEventBindingService();
			mockDesignerLoaderHost.AddService(typeof(IEventBindingService), mockEventBindingService);
			
			System.Console.WriteLine("Before BeginLoad");
			loader.BeginLoad(mockDesignerLoaderHost);
			System.Console.WriteLine("After BeginLoad");
			rootComponent = mockDesignerLoaderHost.RootComponent;
			
			designedForm = new Form();
			designedForm.Name = "NewMainForm";
			mockDesignerLoaderHost.RootComponent = designedForm;
			loader.CallPerformFlush();
			 */
		}
		
		[TestFixtureTearDown]
		public void Dispose()
		{
			// TODO: Add tear down code.
		}
	}
}
