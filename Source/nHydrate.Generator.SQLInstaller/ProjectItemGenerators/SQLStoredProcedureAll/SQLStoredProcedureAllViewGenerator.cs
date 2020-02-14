#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using nHydrate.Generator.ProjectItemGenerators;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.SQLStoredProcedureAll
{
    [GeneratorItem("SQLStoredProcedureAllViewGenerator", typeof(DatabaseProjectGenerator))]
    public class SQLStoredProcedureAllViewGenerator : BaseDbScriptGenerator
    {
        #region Properties

        private string ParentItemPath => @"5_Programmability\Views\Model";

        #endregion

        #region Overrides

        public override int FileCount => 1;

        public override void Generate()
        {
            try
            {
                //Process views
                var sb = new StringBuilder();
                sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
                sb.AppendLine();

                //Defined views
                var grantSB = new StringBuilder();
                foreach (var view in _model.Database.CustomViews.Where(x => x.Generated).OrderBy(x => x.Name))
                {
                    var template = new SQLStoredProcedureViewAllTemplate(_model, view, true, grantSB);
                    sb.Append(template.FileContent);
                }

                //Add grants
                sb.Append(grantSB.ToString());

                var eventArgs = new ProjectItemGeneratedEventArgs("Views.sql", sb.ToString(), ProjectName,
                    this.ParentItemPath, ProjectItemType.Folder, this, true);
                eventArgs.Properties.Add("BuildAction", 3);
                OnProjectItemGenerated(this, eventArgs);

                var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
                OnGenerationComplete(this, gcEventArgs);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

    }
}