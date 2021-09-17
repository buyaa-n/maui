using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Essentials;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls
{
	public static class Device
	{
		[Obsolete("Use Essentials.DevicePlatform.iOS instead.")]
		public static readonly DevicePlatform iOS = DevicePlatform.iOS;
		[Obsolete("Use Essentials.DevicePlatform.Android instead.")]
		public static readonly DevicePlatform Android = DevicePlatform.Android;
		[Obsolete("Use Essentials.DevicePlatform.Windows instead.")]
		public static readonly DevicePlatform UWP = DevicePlatform.UWP;
		[Obsolete("Use Essentials.DevicePlatform.macOS instead.")]
		public static readonly DevicePlatform macOS = DevicePlatform.macOS;
		[Obsolete("Use Essentials.DevicePlatform instead.")]
		public static readonly DevicePlatform GTK = DevicePlatform.Create("GTK");
		[Obsolete("Use Essentials.DevicePlatform.Tizen instead.")]
		public static readonly DevicePlatform Tizen = DevicePlatform.Tizen;
		[Obsolete("Use Essentials.DevicePlatform.Windows instead.")]
		public static readonly DevicePlatform WPF = DevicePlatform.Create("WPF");

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static Internals.DeviceInfo info;

		static IPlatformServices s_platformServices;

		[Obsolete("Use Essentials.DeviceInfo.Idiom instead.")]
		public static DeviceIdiom Idiom => Essentials.DeviceInfo.Idiom;

		[Obsolete("Use Essentials.DeviceInfo.Platform instead.")]
		public static DevicePlatform RuntimePlatform => Essentials.DeviceInfo.Platform;

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static Internals.DeviceInfo Info
		{
			get
			{
				if (info == null)
					throw new InvalidOperationException("You must call Microsoft.Maui.Controls.Forms.Init(); prior to using this property.");
				return info;
			}
			set { info = value; }
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void SetFlowDirection(FlowDirection value) => FlowDirection = value;
		public static FlowDirection FlowDirection { get; internal set; }

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool IsInvokeRequired
		{
			get { return PlatformServices.IsInvokeRequired; }
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static IPlatformServices PlatformServices
		{
			get
			{
				if (s_platformServices == null)
					throw new InvalidOperationException("You must call Microsoft.Maui.Controls.Forms.Init(); prior to using this property.");
				return s_platformServices;
			}
			set
			{
				s_platformServices = value;
				if (s_platformServices != null)
					Application.Current?.PlatformServicesSet();
			}
		}

		public static IPlatformInvalidate PlatformInvalidator { get; set; }

		public static void BeginInvokeOnMainThread(Action action)
		{
			PlatformServices.BeginInvokeOnMainThread(action);
		}

		public static Task<T> InvokeOnMainThreadAsync<T>(Func<T> func)
		{
			var tcs = new TaskCompletionSource<T>();
			BeginInvokeOnMainThread(() =>
			{
				try
				{
					var result = func();
					tcs.SetResult(result);
				}
				catch (Exception ex)
				{
					tcs.SetException(ex);
				}
			});
			return tcs.Task;
		}

		public static Task InvokeOnMainThreadAsync(Action action)
		{
			object wrapAction()
			{ action(); return null; }
			return InvokeOnMainThreadAsync((Func<object>)wrapAction);
		}

		public static Task<T> InvokeOnMainThreadAsync<T>(Func<Task<T>> funcTask)
		{
			var tcs = new TaskCompletionSource<T>();
			BeginInvokeOnMainThread(
				async () =>
				{
					try
					{
						var ret = await funcTask().ConfigureAwait(false);
						tcs.SetResult(ret);
					}
					catch (Exception e)
					{
						tcs.SetException(e);
					}
				}
			);

			return tcs.Task;
		}

		public static Task InvokeOnMainThreadAsync(Func<Task> funcTask)
		{
			async Task<object> wrapFunction()
			{ await funcTask().ConfigureAwait(false); return null; }
			return InvokeOnMainThreadAsync(wrapFunction);
		}

		public static async Task<SynchronizationContext> GetMainThreadSynchronizationContextAsync()
		{
			SynchronizationContext ret = null;
			await InvokeOnMainThreadAsync(() =>
				ret = SynchronizationContext.Current
			).ConfigureAwait(false);
			return ret;
		}

		public static double GetNamedSize(NamedSize size, Element targetElement)
		{
			return GetNamedSize(size, targetElement.GetType());
		}

		public static double GetNamedSize(NamedSize size, Type targetElementType)
		{
			return GetNamedSize(size, targetElementType, false);
		}

		public static void StartTimer(TimeSpan interval, Func<bool> callback)
		{
			PlatformServices.StartTimer(interval, callback);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static Assembly[] GetAssemblies()
		{
			return AppDomain.CurrentDomain.GetAssemblies();
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static double GetNamedSize(NamedSize size, Type targetElementType, bool useOldSizes)
		{
			return PlatformServices.GetNamedSize(size, targetElementType, useOldSizes);
		}

		public static Color GetNamedColor(string name)
		{
			return PlatformServices.GetNamedColor(name);
		}

		internal static Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken)
		{
			return PlatformServices.GetStreamAsync(uri, cancellationToken);
		}

		public static class Styles
		{
			public static readonly string TitleStyleKey = "TitleStyle";

			public static readonly string SubtitleStyleKey = "SubtitleStyle";

			public static readonly string BodyStyleKey = "BodyStyle";

			public static readonly string ListItemTextStyleKey = "ListItemTextStyle";

			public static readonly string ListItemDetailTextStyleKey = "ListItemDetailTextStyle";

			public static readonly string CaptionStyleKey = "CaptionStyle";

			public static readonly Style TitleStyle = new Style(typeof(Label)) { BaseResourceKey = TitleStyleKey };

			public static readonly Style SubtitleStyle = new Style(typeof(Label)) { BaseResourceKey = SubtitleStyleKey };

			public static readonly Style BodyStyle = new Style(typeof(Label)) { BaseResourceKey = BodyStyleKey };

			public static readonly Style ListItemTextStyle = new Style(typeof(Label)) { BaseResourceKey = ListItemTextStyleKey };

			public static readonly Style ListItemDetailTextStyle = new Style(typeof(Label)) { BaseResourceKey = ListItemDetailTextStyleKey };

			public static readonly Style CaptionStyle = new Style(typeof(Label)) { BaseResourceKey = CaptionStyleKey };
		}

		public static void Invalidate(VisualElement visualElement)
		{
			PlatformInvalidator?.Invalidate(visualElement);
		}
	}
}
