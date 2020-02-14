#pragma warning disable 0168
using System;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.Functions
{
	[GeneratorItem("Functions", typeof(DatabaseProjectGenerator))]
	public class FunctionsGenerator : BaseDbScriptGenerator
	{
		#region Properties

		private string ParentItemPath => @"5_Programmability\Functions\Model";

        #endregion

        #region Overrides

        public override int FileCount => 1;

        public override void Generate()
		{
			try
			{
				var template = new FunctionsTemplate(_model);
				var fullFileName = template.FileName;
				var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, this.ParentItemPath, ProjectItemType.Folder, this, true);
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
