using System.Linq;
using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;
using System.Collections.Generic;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ComplexTypes
{
    [GeneratorItem("ComplexTypesGeneratedGenerator", typeof(ComplexTypesExtenderGenerator))]
    public class ComplexTypesGeneratedGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        #region Class Members

        private const string RELATIVE_OUTPUT_LOCATION = @"\Entity\";

        #endregion

        #region Overrides

        public override int FileCount
        {
            //get { return GetListSP().Count + GetListFunc().Count; }
            get { return GetListSP().Count; }
        }

        private List<CustomStoredProcedure> GetListSP()
        {
            return _model.Database.CustomStoredProcedures
                .Where(x => x.Generated && x.GeneratedColumns.Count > 0)
                .OrderBy(x => x.Name)
                .ToList();
        }

        //private List<Function> GetListFunc()
        //{
        //    return _model.Database.Functions
        //        .Where(x => x.Generated && x.IsTable)
        //        .OrderBy(x => x.Name)
        //        .ToList();
        //}

        public override void Generate()
        {
            foreach (var item in GetListSP())
            {
                var template = new ComplexTypesSPGeneratedTemplate(_model, item);
                var fullParentName = RELATIVE_OUTPUT_LOCATION + template.ParentItemName;
                var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, fullParentName, this, true);
                OnProjectItemGenerated(this, eventArgs);
            }

            //foreach (var item in GetListFunc())
            //{
            //    var template = new ComplexTypesFuncGeneratedTemplate(_model, item);
            //    var fullParentName = RELATIVE_OUTPUT_LOCATION + template.ParentItemName;
            //    var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, fullParentName, this, true);
            //    OnProjectItemGenerated(this, eventArgs);
            //}

            //Process deleted items
            foreach (var name in _model.RemovedStoredProcedures)
            {
                var fullFileName = RELATIVE_OUTPUT_LOCATION + name + ".Generated.cs";
                var eventArgs = new ProjectItemDeletedEventArgs(fullFileName, ProjectName, this);
                OnProjectItemDeleted(this, eventArgs);
            }

            ////Process deleted items
            //foreach (var name in _model.RemovedFunctions)
            //{
            //    var fullFileName = RELATIVE_OUTPUT_LOCATION + name + ".Generated.cs";
            //    var eventArgs = new ProjectItemDeletedEventArgs(fullFileName, ProjectName, this);
            //    OnProjectItemDeleted(this, eventArgs);
            //}

            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

        #endregion

    }
}
