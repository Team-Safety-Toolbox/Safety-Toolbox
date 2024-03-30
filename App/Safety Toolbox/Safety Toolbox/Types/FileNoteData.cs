using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safety_Toolbox.Types
{
    class FileNoteData
    {
        public DateTime NoteDate { get; set; }
        public string NoteFileName { get; set; }

        public FileNoteData(){}

        public FileNoteData(DateTime noteDate, string noteFileName)
        {
            NoteDate = noteDate;
            NoteFileName = noteFileName;
        }
    }
}
