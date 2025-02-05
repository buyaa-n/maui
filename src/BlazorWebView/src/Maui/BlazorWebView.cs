﻿using System;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.FileProviders;

namespace Microsoft.AspNetCore.Components.WebView.Maui
{
	public class BlazorWebView : Microsoft.Maui.Controls.View, IBlazorWebView
	{
		internal const string AppHostAddress = "0.0.0.0";

		private readonly JSComponentConfigurationStore _jSComponents = new();

		public BlazorWebView()
		{
			RootComponents = new RootComponentsCollection(_jSComponents);
		}

		JSComponentConfigurationStore IBlazorWebView.JSComponents => _jSComponents;

		public string? HostPage { get; set; }

		public RootComponentsCollection RootComponents { get; }

		/// <inheritdoc/>
		public event EventHandler<ExternalLinkNavigationEventArgs>? ExternalNavigationStarting;

		/// <inheritdoc/>
		public virtual IFileProvider CreateFileProvider(string contentRootDir)
		{
			// Call into the platform-specific code to get that platform's asset file provider
			return ((BlazorWebViewHandler)(Handler!)).CreateFileProvider(contentRootDir);
		}

		internal void NotifyExternalNavigationStarting(ExternalLinkNavigationEventArgs args)
		{
			ExternalNavigationStarting?.Invoke(this, args);
		}
	}
}
