using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using nHydrate.Generator.EFCodeFirstNetCore;
using nHydrate.Generator.ProjectItemGenerators;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ContextStartup
{
    [GeneratorItem("ContextStartupGeneratedGenerator", typeof(ContextStartupExtenderGenerator))]
    public class ContextStartupGeneratedGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        #region Class Members

        private const string RELATIVE_OUTPUT_LOCATION = @"\";

        #endregion

        #region Overrides

        public override int FileCount
        {
            get { return 1; }
        }

        public override void Generate()
        {
            ContextStartupGeneratedTemplate template = new ContextStartupGeneratedTemplate(_model);
            string fullParentName = RELATIVE_OUTPUT_LOCATION + template.ParentItemName;
            ProjectItemGeneratedEventArgs eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, fullParentName, this, true);
            OnProjectItemGenerated(this, eventArgs);
            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

        #endregion

    }
}
