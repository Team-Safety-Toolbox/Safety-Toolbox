using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safety_Toolbox.Types
{
    class TextNoteData
    {
        public DateTime NoteDate { get; set; }
        public string NoteContent { get; set; }

        public TextNoteData() {}

        public TextNoteData(DateTime noteDate, string noteContent)
        {
            NoteDate = noteDate;
            NoteContent = noteContent;
        }

    }
}
