using System;
//+
namespace Squid.Web.Controls
{
    internal class InfoBlockData
    {
        //- @BlockHeight -//
        public Int32 BlockHeight { get; set; }

        //- @BlockSize -//
        public BlockSize BlockSize { get; set; }

        //- @BlockType -//
        public BlockType BlockType { get; set; }

        //- @BlockWidth -//
        public Int32 BlockWidth { get; set; }

        //- @DataSource -//
        public String DataSource { get; set; }

        //- @ShowBorder -//
        public Boolean ShowBorder { get; set; }

        //+
        //- @Ctor -//
        public InfoBlockData()
        {
            this.BlockSize = BlockSize.Regular;
            this.ShowBorder = true;
        }

        //- @Ctor -//
        public InfoBlockData(BlockSize size)
        {
            this.BlockSize = size;
            this.ShowBorder = true;
        }

        //- @Ctor -//
        public InfoBlockData(BlockSize size, Boolean showBorder)
        {
            this.BlockSize = size;
            this.ShowBorder = showBorder;
        }
    }
}