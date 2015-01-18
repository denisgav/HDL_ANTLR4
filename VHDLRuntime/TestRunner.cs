//
//  Copyright (C) 2010-2014  Denis Gavrish
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using VHDLRuntime.ValueDump;

namespace VHDLRuntime
{
    public static class TestRunner
    {
        public static void LoadMyAssemblyAndRun(string path, string assemblyName, string architecture, Logger logger, string vcdPath)
        {
            AppDomain appDomain;
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationName = "Plugins";
            setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            setup.PrivateBinPath =
                Path.GetDirectoryName(path).Substring(
                Path.GetDirectoryName(path).LastIndexOf(
                Path.DirectorySeparatorChar) + 1);
            setup.CachePath = Path.Combine(path,
                "cache" + Path.DirectorySeparatorChar);
            setup.ShadowCopyFiles = "true";
            setup.ShadowCopyDirectories = path;

            appDomain = AppDomain.CreateDomain(
                "Plugins", null, setup);


            object instance = appDomain.CreateInstanceAndUnwrap(
                assemblyName,
                string.Format("Work.{0}", architecture));

            ExecuteFunction(instance, logger, vcdPath);

            AppDomain.Unload(appDomain);
        }

        public static void ExecuteFunction(object instance, Logger logger, string vcdPath)
        {
            ArchitectureBase runnable = (instance as ArchitectureBase);
            runnable.Logger = logger;
            runnable.MainFunction();
            runnable.Logger.Flush();

            SimulationScope root = runnable.GetSimulationScope(null);
            VCDWriter writer = new VCDWriter(root, vcdPath);
            writer.Write();
        }
    }
}
