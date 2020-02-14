using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using nHydrate.Generator.ProjectItemGenerators;
using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Datasite
{
	[GeneratorItem("Datasite", typeof(DatasiteProjectGenerator))]
	public class DatasiteGenerator : BaseScriptGenerator
	{
		#region Class Members

		private const string EMBEDDED_LOCATION = "EmbeddedClasses";
		private const string PARENT_ITEM_NAME = @"";

		#endregion

		#region Overrides

		public override int FileCount
		{
			get
			{
				return _model.Database.Tables.Count(x => x.Generated) +
					_model.Database.CustomViews.Count(x => x.Generated) +
					_model.Database.CustomStoredProcedures.Count(x => x.Generated) +
					_model.Database.Functions.Count(x => x.Generated) +
					9;
			}
		}
		
		public override void Generate()
		{
			//var template = new DatasiteOverviewTemplate(_model);
			//var fullFileName = template.FileName;
			//var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
			//OnProjectItemGenerated(this, eventArgs);
			//var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
			//OnGenerationComplete(this, gcEventArgs);

			#region Images
			{ //FK
				var eventArgs = new ProjectItemGeneratedEventArgs("fk.gif", string.Empty, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true)
				{
					ProjectItemBinaryContent = Helpers.GetFileBinContent(new EmbeddedResourceName(GetEmbeddedPath() + ".fk.gif")),
					ContentType = ProjectItemContentType.Binary,
				};
				OnProjectItemGenerated(this, eventArgs);
				var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}
			{ //PK
				var eventArgs = new ProjectItemGeneratedEventArgs("key.gif", string.Empty, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true)
				{
					ProjectItemBinaryContent = Helpers.GetFileBinContent(new EmbeddedResourceName(GetEmbeddedPath() + ".key.gif")),
					ContentType = ProjectItemContentType.Binary,
				};
				OnProjectItemGenerated(this, eventArgs);
				var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}
			{ //YES
				var eventArgs = new ProjectItemGeneratedEventArgs("yes.gif", string.Empty, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true)
				{
					ProjectItemBinaryContent = Helpers.GetFileBinContent(new EmbeddedResourceName(GetEmbeddedPath() + ".yes.gif")),
					ContentType = ProjectItemContentType.Binary,
				};
				OnProjectItemGenerated(this, eventArgs);
				var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}
			{ //NO
				var eventArgs = new ProjectItemGeneratedEventArgs("no.gif", string.Empty, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true)
				{
					ProjectItemBinaryContent = Helpers.GetFileBinContent(new EmbeddedResourceName(GetEmbeddedPath() + ".no.gif")),
					ContentType = ProjectItemContentType.Binary,
				};
				OnProjectItemGenerated(this, eventArgs);
				var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}
			{ //CONSTRAINT
				var eventArgs = new ProjectItemGeneratedEventArgs("constraint.gif", string.Empty, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true)
				{
					ProjectItemBinaryContent = Helpers.GetFileBinContent(new EmbeddedResourceName(GetEmbeddedPath() + ".constraint.gif")),
					ContentType = ProjectItemContentType.Binary,
				};
				OnProjectItemGenerated(this, eventArgs);
				var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}
			{ //INDEX
				var eventArgs = new ProjectItemGeneratedEventArgs("index.gif", string.Empty, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true)
				{
					ProjectItemBinaryContent = Helpers.GetFileBinContent(new EmbeddedResourceName(GetEmbeddedPath() + ".index.gif")),
					ContentType = ProjectItemContentType.Binary,
				};
				OnProjectItemGenerated(this, eventArgs);
				var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}
			#endregion

			//Style sheet
			{
				var template = new StyleSheetTemplate(_model, GetEmbeddedPath());
				var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
				OnProjectItemGenerated(this, eventArgs);
				var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}

			//Site overview
			{
				var template = new DatasiteOverviewTemplate(_model, GetEmbeddedPath());
				var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
				OnProjectItemGenerated(this, eventArgs);
				var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}

			//Table overview
			{
				var template = new DatasiteTableListTemplate(_model, GetEmbeddedPath());
				var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
				OnProjectItemGenerated(this, eventArgs);
				var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}

			//Tables
			foreach (var item in _model.Database.Tables.Where(x => x.Generated).OrderBy(x => x.Name))
			{
				var template = new DatasiteTableItemTemplate(_model, item, GetEmbeddedPath());
				var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
				OnProjectItemGenerated(this, eventArgs);
				var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}

			//Views overview
			{
				var template = new DatasiteViewListTemplate(_model, GetEmbeddedPath());
				var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
				OnProjectItemGenerated(this, eventArgs);
				var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}

			//Views
			foreach (var item in _model.Database.CustomViews.Where(x => x.Generated).OrderBy(x => x.Name))
			{
				var template = new DatasiteViewItemTemplate(_model, item, GetEmbeddedPath());
				var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
				OnProjectItemGenerated(this, eventArgs);
				var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}

			//Stored Procs overview
			{
				var template = new DatasiteStoredProcListTemplate(_model, GetEmbeddedPath());
				var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
				OnProjectItemGenerated(this, eventArgs);
				var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}

			//Stored Procs
			foreach (var item in _model.Database.CustomStoredProcedures.Where(x => x.Generated).OrderBy(x => x.Name))
			{
				var template = new DatasiteStoredProcItemTemplate(_model, item, GetEmbeddedPath());
				var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
				OnProjectItemGenerated(this, eventArgs);
				var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}

			//Functions overview
			{
				var template = new DatasiteFunctionListTemplate(_model, GetEmbeddedPath());
				var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
				OnProjectItemGenerated(this, eventArgs);
				var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}

			//Functions
			foreach (var item in _model.Database.Functions.Where(x => x.Generated).OrderBy(x => x.Name))
			{
				var template = new DatasiteFunctionItemTemplate(_model, item, GetEmbeddedPath());
				var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
				OnProjectItemGenerated(this, eventArgs);
				var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}

			//Scripts
			{
				var template1 = new DatasiteJQueryTemplate(_model, GetEmbeddedPath());
				var eventArgs = new ProjectItemGeneratedEventArgs(template1.FileName, template1.FileContent, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
				OnProjectItemGenerated(this, eventArgs);
				var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);

				var template2 = new DatasiteJQueryCornersTemplate(_model, GetEmbeddedPath());
				eventArgs = new ProjectItemGeneratedEventArgs(template2.FileName, template2.FileContent, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
				OnProjectItemGenerated(this, eventArgs);
				gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);

				var template3 = new DatasiteMasterScriptTemplate(_model, GetEmbeddedPath());
				eventArgs = new ProjectItemGeneratedEventArgs(template3.FileName, template3.FileContent, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
				OnProjectItemGenerated(this, eventArgs);
				gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}

		}

		#endregion

		private string GetEmbeddedPath()
		{
			return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "." + EMBEDDED_LOCATION;
		}

	}
}

