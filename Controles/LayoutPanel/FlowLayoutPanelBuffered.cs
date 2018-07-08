using System.Windows.Forms;

namespace Bot_Dofus_1._29._1.Controles.LayoutPanel
{
    class FlowLayoutPanelBuffered : FlowLayoutPanel
    {
        public FlowLayoutPanelBuffered()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
    }
}
