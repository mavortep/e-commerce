using Domain.Abstract;
using Domain.Concrete;
using Domain.Entities;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyFirstWebApplication.Infrastruscture
{
	public class NinjectDependencyResolver : IDependencyResolver
	{
		private IKernel kernel;

		public NinjectDependencyResolver(IKernel kernelParam)
		{
			kernel = kernelParam;
			AddBindings();
		}

		private void AddBindings()
		{
			kernel.Bind<IDishRepository>().To<EFDishRepository>();

			EmailSettings emailSettings = new EmailSettings
			{
				WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
			};

			kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
				.WithConstructorArgument("settings", emailSettings);
		}

		public object GetService(Type serviceType)
		{
			return kernel.TryGet(serviceType);
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			return kernel.GetAll(serviceType);
		}
	}
}