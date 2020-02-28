using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL.SubAreaFileBehavior
{
    public interface ISubAreaFileBehaviorBase
    {
       bool Add(IEnumerable<SubAreaFile> subAreaFiles, ref string message);
       bool AddValidates(SubAreaFile subAreaFile, ref string message);
       bool Edit(IEnumerable<SubAreaFile>subAreaFiles, ref string message);
       bool EditValidates(SubAreaFile subAreaFile, ref string message);
       bool Delete(IEnumerable<Guid>ids, ref string message);
       bool DeleteValidates(SubAreaFile subAreaFile, ref string message);
       bool Move(IEnumerable<SubAreaFile> sourceSubAreaFiles, SubAreaFile targetSubAreaFile, ref string message);
       bool MoveValidates(SubAreaFile sourceSubAreaFile, SubAreaFile targetSubAreaFile, ref string message);
       bool Validates(SubAreaFile subAreaFile, ref string message);
    }
}
