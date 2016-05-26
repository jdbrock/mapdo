using System;
using System.Reflection;
using System.Collections.Specialized;

namespace Mapdo
{
	public class NotifyCollectionChangedEventSubscription : WeakEventSubscription<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>
	{
		private static readonly EventInfo EventInfo = typeof (INotifyCollectionChanged).GetTypeInfo().GetDeclaredEvent("CollectionChanged");

		// This code ensures the CollectionChanged event is not stripped by Xamarin linker
		// see https://github.com/MvvmCross/MvvmCross/pull/453
		public static void LinkerPleaseInclude(INotifyCollectionChanged collection)
		{
			collection.CollectionChanged += (sender, e) => { };
		}

		public NotifyCollectionChangedEventSubscription(INotifyCollectionChanged source,
		                                                   EventHandler<NotifyCollectionChangedEventArgs>
		                                                   targetEventHandler)
			: base(source, EventInfo, targetEventHandler)
		{
		}

		protected override Delegate CreateEventHandler()
		{
			return new NotifyCollectionChangedEventHandler(OnSourceEvent);
		}
	}
}

