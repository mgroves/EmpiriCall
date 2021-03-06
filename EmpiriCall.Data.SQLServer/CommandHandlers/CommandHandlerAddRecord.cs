﻿using System.Collections.Generic;
using System.Linq;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Data.SQLServer.CommandHandlers
{
    public class CommandHandlerAddRecord : ICommandHandler<CommandAddRecord>
    {
        readonly EmpiriCallDbContext _context;

        public CommandHandlerAddRecord(EmpiriCallDbContext context)
        {
            _context = context;
        }

        public void Handle(CommandAddRecord command)
        {
            var metaData = _context.MetaData
                    .OrderByDescending(m => m.Version)
                    .First();

            var action = metaData.ActionInfo
                    .Where(a => a.ActionName == command.ActionName)
                    .Where(a => a.ControllerName == command.ControllerName)
                    .Where(a => ParameterBasicInfo.AreTheSame(command.ParameterInfo, a.ParameterInfo))
                    .First();
            
            if (action.CallRecords == null)
                action.CallRecords = new List<DetailRecord>();

            action.CallRecords.Add(new DetailRecord
            {
                CustomValues = command.CustomValues,
                UserName = command.UserName,
                TimeStamp = command.TimeStamp
            });
            _context.SaveChanges();
        }
    }
}