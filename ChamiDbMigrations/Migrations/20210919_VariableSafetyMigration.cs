using System;
using System.Collections.Generic;
using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(202109190001)]
    public class VariableSafetyMigration : Migration
    {
        private static List<EnvironmentVariableBlacklist> GetInitialVariables()
        {
            var entitiesToAdd = new List<EnvironmentVariableBlacklist>();
            int id = 1;

            var path = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "PATH",
                InitialValue = null,
                IsWindowsDefault = true, 
                IsEnabled = true
            };
            entitiesToAdd.Add(path);

            id++;
            var allUsersProfile = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "ALLUSERSPROFILE",
                InitialValue = "C:\\ProgramData",
                IsWindowsDefault = true,
                IsEnabled = true
            };
            entitiesToAdd.Add(allUsersProfile);

            id++;
            var appData = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "APPDATA",
                InitialValue = "C:\\Users\\%USERNAME%\\AppData\\Roaming",
                IsWindowsDefault = true,
                IsEnabled = true
            };
            entitiesToAdd.Add(appData);

            id++;
            var commonProgramFiles = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "CommonProgramFiles",
                InitialValue = "C:\\Program Files\\Common Files",
                IsWindowsDefault = true,
                IsEnabled = true
            };
            entitiesToAdd.Add(commonProgramFiles);

            id++;
            var commonProgramFilesX86 = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "CommonProgramFiles(x86)",
                InitialValue = "C:\\Program Files (x86)\\Common Files",
                IsWindowsDefault = true,
                IsEnabled = true
            };
            entitiesToAdd.Add(commonProgramFilesX86);

            id++;
            var commonProgramW6432 = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "CommonProgramW6432",
                InitialValue = "C:\\Program Files\\Common Files",
                IsWindowsDefault = true, 
                IsEnabled = true
            };
            entitiesToAdd.Add(commonProgramW6432);

            id++;
            var computerName = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "COMPUTERNAME",
                InitialValue = null,
                IsWindowsDefault = true,
                IsEnabled = true
            };
            entitiesToAdd.Add(computerName);

            id++;
            var comSpec = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "ComSpec",
                InitialValue = "C:\\Windows\\System32\\cmd.exe",
                IsWindowsDefault = true,
                IsEnabled = true
            };
            entitiesToAdd.Add(comSpec);

            id++;
            var homeDrive = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "HOMEDRIVE",
                InitialValue = "C:",
                IsWindowsDefault = true, 
                IsEnabled = true
            };
            entitiesToAdd.Add(homeDrive);

            id++;
            var homePath = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "HOMEPATH",
                InitialValue = "\\Users\\%USERNAME%",
                IsWindowsDefault = true, 
                IsEnabled = true
            };
            entitiesToAdd.Add(homePath);

            id++;
            var localAppData = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "LOCALAPPDATA",
                InitialValue = "C:\\Users\\%USERNAME%\\AppData\\Local",
                IsWindowsDefault = true, 
                IsEnabled = true
            };
            entitiesToAdd.Add(localAppData);

            id++;
            var logonServer = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "LOGONSERVER",
                InitialValue = null,
                IsWindowsDefault = true,
                IsEnabled = true
            };
            entitiesToAdd.Add(logonServer);

            id++;
            var pathExt = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "PATHEXT",
                InitialValue = ".COM;.EXE;.BAT;.CMD;.VBS;.VBE;.JS;.WSF;.WSH",
                IsWindowsDefault = true, 
                IsEnabled = true
            };
            entitiesToAdd.Add(pathExt);

            id++;
            var programData = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "ProgramData",
                InitialValue = "%SystemDrive%\\ProgramData",
                IsWindowsDefault = true, 
                IsEnabled = true
            };
            entitiesToAdd.Add(programData);

            id++;
            var programFiles = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "ProgramFiles",
                InitialValue = "%SystemDrive%\\Program Files",
                IsWindowsDefault = true, 
                IsEnabled = true
            };

            id++;
            var programFilesX86 = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "ProgramFiles(x86)",
                InitialValue = "%SystemDrive%\\Program Files (x86)",
                IsWindowsDefault = true, 
                IsEnabled = true
            };
            entitiesToAdd.Add(programFilesX86);

            id++;
            var programW6432 = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "ProgramW6432",
                InitialValue = "%SystemDrive%\\Program Files",
                IsWindowsDefault = true, 
                IsEnabled = true
            };
            entitiesToAdd.Add(programW6432);

            id++;
            var prompt = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "PROMPT",
                InitialValue = "$P$G",
                IsWindowsDefault = true,
                IsEnabled = true
            };
            entitiesToAdd.Add(prompt);

            id++;
            var psModulePath = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "PSModulePath",
                InitialValue = "%SystemRoot%\\system32\\WindowsPowerShell\\v1.0\\Modules\\",
                IsWindowsDefault = true, 
                IsEnabled = true
            };
            entitiesToAdd.Add(psModulePath);

            id++;
            var publicVariable = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "PUBLIC",
                InitialValue = "%SystemDrive%\\Users\\Public",
                IsWindowsDefault = true, 
                IsEnabled = true
            };
            entitiesToAdd.Add(publicVariable);

            id++;
            var systemDrive = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "SystemDrive",
                InitialValue = "C:",
                IsWindowsDefault = true, 
                IsEnabled = true
            };
            entitiesToAdd.Add(systemDrive);

            id++;
            var systemRoot = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "SystemRoot",
                InitialValue = "C:\\Windows",
                IsWindowsDefault = true, 
                IsEnabled = true
            };
            entitiesToAdd.Add(systemRoot);

            id++;
            var temp = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "TEMP",
                InitialValue = "%SystemRoot%\\TEMP",
                IsWindowsDefault = true, 
                IsEnabled = true
            };
            entitiesToAdd.Add(temp);
            id++;     
            var tmp = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "TMP",
                InitialValue = "%SystemRoot%\\TEMP",
                IsWindowsDefault = true, 
                IsEnabled = true
            };
            entitiesToAdd.Add(tmp);
            
            id++;
            var userDomain = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "USERDOMAIN",
                InitialValue = null,
                IsWindowsDefault = true, 
                IsEnabled = true
            };
            entitiesToAdd.Add(userDomain);

            id++;
            var username = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "USERNAME",
                InitialValue = null,
                IsWindowsDefault = true, 
                IsEnabled = true
            };
            entitiesToAdd.Add(username);

            id++;
            var userProfile = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "USERPROFILE",
                InitialValue = "%SystemDrive%\\Users\\%USERNAME%",
                IsWindowsDefault = true, 
                IsEnabled = true
            };
            entitiesToAdd.Add(userProfile);

            id++;
            var windir = new EnvironmentVariableBlacklist()
            {
                Id = id,
                Name = "windir",
                InitialValue = "%SystemDrive%\\WINDOWS",
                IsWindowsDefault = true, 
                IsEnabled = true
            };
            entitiesToAdd.Add(windir);
            
            return entitiesToAdd;
        }
        public override void Up()
        {
            Create.Table("EnvironmentVariableBlacklist")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("InitialValue").AsString().Nullable()
                .WithColumn("IsWindowsDefault").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("IsEnabled").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("AddedOn").AsDateTime().NotNullable().WithDefaultValue(DateTime.MinValue);

            var entitiesToAdd = GetInitialVariables();
            foreach (var variable in entitiesToAdd)
            {
                Insert.IntoTable("EnvironmentVariableBlacklist").Row(variable);
            }
        }

        public override void Down()
        {
            Delete.Table("EnvironmentVariableBlacklist");
        }
    }
}