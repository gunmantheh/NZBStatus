using System.Collections.Generic;
using NZBStatus.DTOs;

namespace NZBStatus
{
    public class QueueSelect
    {
        private IList<Slot> _list;
        private int _position;
        public QueueSelect()
        {
            _list = new List<Slot>();
            _position = 0; // represents current download
        }

        private int NumOfRecords {
            get { return _list.Count; }
        }

        public int Position 
        {
            get { return _position; }
        }

        public int Up()
        {
            _position -= _position == 0? 0: 1;
            return _position;
        }

        public int Down()
        {
            _position += _position == NumOfRecords ? 0 : 1;
            return _position;
        }

        public void Refresh(IList<Slot> list)
        {
            _list = list;
            if (list.Count < _position)
            {
                _position = list.Count;
            }
        }
    }
}