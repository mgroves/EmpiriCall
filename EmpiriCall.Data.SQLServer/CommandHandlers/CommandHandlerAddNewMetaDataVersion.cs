﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Data.SQLServer.CommandHandlers
{
    public class CommandHandlerAddNewMetaDataVersion : ICommandHandler<CommandAddNewMetaDataVersion>
    {
        readonly EmpiriCallDbContext _context;

        public CommandHandlerAddNewMetaDataVersion(EmpiriCallDbContext context)
        {
            _context = context;
        }

        public void Handle(CommandAddNewMetaDataVersion command)
        {
            var latestMeta = _context.MetaData.OrderByDescending(m => m.Version).FirstOrDefault();

            var version = 1;
            if (latestMeta != null)
                version = latestMeta.Version + 1;

            var meta = new MetaData {Version = version};

            UpdateMeta(meta);
            _context.MetaData.Add(meta);
            _context.SaveChanges();
        }

        void UpdateMeta(MetaData meta)
        {
            meta.LastUpdated = DateTime.Now;
            meta.ActionInfo = new List<ActionInfo>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                try
                {
                    var controllers = assembly.GetTypes()
                        .Where(t => t.IsSubclassOf(typeof(Controller)))
                        .ToList();
                    meta.ActionInfo.AddRange(MapActionInfo(controllers));
                }
                catch
                {
                    // exception when trying to load certain assemblies
                    // not sure how to prevent attempting to load them
                    // they are framework/.net type assemblies
                    // that I'm not interested in checking out anyway!
                }
            }
        }

        IEnumerable<ActionInfo> MapActionInfo(List<Type> controllers)
        {
            var list = new List<ActionInfo>();
            foreach (var controller in controllers)
            {
	            var actions = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
					.ToList();

				foreach (var action in actions)
				{
					if (ExcludeAction(action))
						continue;
                    var actionInfo = new ActionInfo();
                    actionInfo.ControllerName = controller.FullName;
                    actionInfo.ActionName = action.Name;
                    actionInfo.ParameterInfo = action.GetParameters().Select(p =>
                        new ParameterBasicInfo
                        {
                            ParameterName = p.Name,
                            ParameterTypeFullName = p.ParameterType.FullName
                        }).ToList();
                    list.Add(actionInfo);
                }
            }
            return list;
        }

		// don't return certain members; this currently means:
		// - autogenerated setters
		// - autogenerated getters
	    private bool ExcludeAction(MethodInfo action)
	    {
			if (action.Name.StartsWith("set_"))
				return true;
			if (action.Name.StartsWith("get_"))
				return true;
		    return false;
	    }
    }
}