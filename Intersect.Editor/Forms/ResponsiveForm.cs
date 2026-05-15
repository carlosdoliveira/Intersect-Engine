namespace Intersect.Editor.Forms;

public class ResponsiveForm : Form
{

    protected ResponsiveForm()
    {
        Load += (_, _) => ApplyResponsiveBehavior();
    }

    protected virtual bool EnableAutomaticResize => true;

    protected virtual bool PreserveToolWindowChrome => true;

    private void ApplyResponsiveBehavior()
    {
        if (!EnableAutomaticResize)
        {
            return;
        }

        if (
            FormBorderStyle != FormBorderStyle.FixedSingle &&
            FormBorderStyle != FormBorderStyle.FixedDialog &&
            FormBorderStyle != FormBorderStyle.Fixed3D &&
            FormBorderStyle != FormBorderStyle.FixedToolWindow
        )
        {
            return;
        }

        FormBorderStyle = FormBorderStyle == FormBorderStyle.FixedToolWindow
            ? PreserveToolWindowChrome
                ? FormBorderStyle.SizableToolWindow
                : FormBorderStyle.Sizable
            : FormBorderStyle.Sizable;
    }

}
