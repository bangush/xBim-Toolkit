﻿using System;
using System.Collections.Generic;
using System.IO;
using Xbim.XbimExtensions.Interfaces;

namespace XbimExtract
{
    public class Params
    {
        public string SourceModelName;
        public string TargetModelName;
        public List<uint> EntityLabels;
        public bool IsValid { get; set; }
        public bool SourceIsXbimFile { get; set; }
        /// <summary>
        /// Include project and other context objects to create a vlaid schema (ish)
        /// </summary>
        public bool IncludeContext { get; set; }


        public static Params ParseParams(string[] args)
        {
            Params result = new Params(args);

            return result;
        }

        private Params(string[] args)
        {

            try
            {
                if (args.Length < 3) throw new Exception("Invalid number of Parameters, 3 required");
                SourceModelName = GetModelFileName(args[0], ".ifc");
                if (!File.Exists(SourceModelName)) throw new Exception(SourceModelName + " does not exist");
                TargetModelName = GetModelFileName(args[1], ".xbim");
                SourceIsXbimFile = Path.GetExtension(SourceModelName).ToLower() == ".xbim";
                EntityLabels = new List<uint>(args.Length - 2);
                for (int i = 2; i < args.Length; i++) EntityLabels.Add(UInt32.Parse(args[i]));
                // Parameters are valid
                IsValid = true;
                IncludeContext = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("XbimExtract SourceModelName TargetModelName 325 [1756 2678]");
                Console.WriteLine("\tModelName extensions supported are .xBIM, .ifc, .ifcxml");
                IsValid = false;
            }
        }

        private string GetModelFileName(string arg, string defaultExtension)
        {
            string extName = Path.GetExtension(arg);
            string fileName = Path.GetFileNameWithoutExtension(arg);
            string dirName = Path.GetDirectoryName(arg);
            if (string.IsNullOrWhiteSpace(dirName))
                dirName = Directory.GetCurrentDirectory();
            if (string.IsNullOrWhiteSpace(extName))
                extName = defaultExtension;
            switch (extName.ToLower())
            {
                case ".xbim":
                case ".ifc":
                case ".ifcxml":
                    return Path.ChangeExtension(Path.Combine(dirName, fileName), extName);
                default:
                    throw new Exception("Invalid file extension (" + extName + ")");
            }
           
        }


       
    }

}
