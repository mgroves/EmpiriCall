using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Data.SQLServer.CommandHandlers
{
    public class CommandHandlerMetaDataUpdate : ICommandHandler<CommandMetaDataUpdate>
    {
        readonly EmpiriCallDbContext _context;

        public CommandHandlerMetaDataUpdate(EmpiriCallDbContext context)
        {
            _context = context;
        }

        public void Handle(CommandMetaDataUpdate command)
        {
            var meta = GetMetaData();

            if (command.ForceUpdate || meta == null)
            {
                meta = meta ?? new MetaData();
                UpdateMeta(meta);
                if (meta.Id == default(int))
                    _context.MetaData.Add(meta);
                _context.SaveChanges();
            }
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
                    meta.ActionInfo = MapActionInfo(controllers).ToList();
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

        MetaData GetMetaData()
        {
            return _context.MetaData.FirstOrDefault();
        }

        IEnumerable<ActionInfo> MapActionInfo(List<Type> controllers)
        {
            var list = new List<ActionInfo>();
            foreach (var controller in controllers)
            {
                foreach (var action in controller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
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
    }
}