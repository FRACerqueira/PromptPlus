namespace PPlus.Objects
{
    public struct PositionResult
    {
        public PositionResult(int rowStart, int colStart, int rowEnd, int colEnd)
        {
            RowStart = rowStart;
            ColStart = colStart;
            RowEnd = rowEnd;
            ColEnd = colEnd;
        }
        public int RowStart { get; private set; }
        public int ColStart { get; private set; }
        public int RowEnd { get; private set; }
        public int ColEnd { get; private set; }

        public void Update(PositionResult position)
        {
            if (position.RowEnd >= RowEnd)
            {
                if (position.RowEnd == RowEnd)
                {

                    if (position.ColEnd >= ColEnd)
                    {
                        ColEnd = position.ColEnd;
                    }
                }
                else
                {
                    RowEnd = position.RowEnd;
                    ColEnd = position.ColEnd;
                }
            }
        }
    }
}
