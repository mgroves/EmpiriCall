using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Db4objects.Db4o;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Data.Db4o.CommandHandlers
{
    public class CommandHandlerAddNewMetaDataVersion : ICommandHandler<CommandAddNewMetaDataVersion>
    {
        readonly string _db4oFilePath;

        public CommandHandlerAddNewMetaDataVersion(string db4oFilePath)
        {
            _db4oFilePath = db4oFilePath;
        }

        public void Handle(CommandAddNewMetaDataVersion command)
        {
            using (var db = Db4oEmbedded.OpenFile(_db4oFilePath))
            {
                var latestMeta = db.Query<MetaData>().OrderByDescending(m => m.Version).FirstOrDefault();

                var version = 1;
                if (latestMeta != null)
                    version = latestMeta.Version + 1;

                var meta = new MetaData {Version = version};
                UpdateMeta(meta);
                db.Store(meta);
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