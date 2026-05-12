namespace Intersect.Editor.Forms;

public class ResponsiveForm : Form
{

    protected ResponsiveForm()
    {
        Load += (_, _) => ApplyResponsiveBehavior();
    }

    private void ApplyResponsiveBehavior()
    {
        if (
            FormBorderStyle != FormBorderStyle.FixedSingle &&
            FormBorderStyle != FormBorderStyle.FixedDialog &&
            FormBorderStyle != FormBorderStyle.Fixed3D &&
            FormBorderStyle != FormBorderStyle.FixedToolWindow
        )
        {
            return;
        }

        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
    }

}
