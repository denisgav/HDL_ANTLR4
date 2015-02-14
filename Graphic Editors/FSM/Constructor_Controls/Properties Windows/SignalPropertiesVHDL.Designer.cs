namespace Schematix.FSM
{
    partial class SignalPropertiesVHDL
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBoxType = new System.Windows.Forms.GroupBox();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.groupBoxIntegerRange = new System.Windows.Forms.GroupBox();
            this.comboBoxIntegerRange2 = new System.Windows.Forms.ComboBox();
            this.comboBoxIntegerRange1 = new System.Windows.Forms.ComboBox();
            this.checkBoxIntegerAvalable = new System.Windows.Forms.CheckBox();
            this.radioButtonIntegerDownTo = new System.Windows.Forms.RadioButton();
            this.radioButtonIntegerTo = new System.Windows.Forms.RadioButton();
            this.groupBoxBusRange = new System.Windows.Forms.GroupBox();
            this.comboBoxBusRange2 = new System.Windows.Forms.ComboBox();
            this.comboBoxBusRange1 = new System.Windows.Forms.ComboBox();
            this.checkBoxBusAvaliable = new System.Windows.Forms.CheckBox();
            this.radioButtonBusDownTo = new System.Windows.Forms.RadioButton();
            this.radioButtonBusTo = new System.Windows.Forms.RadioButton();
            this.radioButtonUserDefined = new System.Windows.Forms.RadioButton();
            this.radioButtonCharacter = new System.Windows.Forms.RadioButton();
            this.radioButtonBoolean = new System.Windows.Forms.RadioButton();
            this.radioButtonInteger = new System.Windows.Forms.RadioButton();
            this.radioButtonBit = new System.Windows.Forms.RadioButton();
            this.radioButtonStd_uLogic = new System.Windows.Forms.RadioButton();
            this.radioButtonStd_Logic = new System.Windows.Forms.RadioButton();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.comboBoxGenType = new System.Windows.Forms.ComboBox();
            this.groupBoxPortProperties = new System.Windows.Forms.GroupBox();
            this.radioButtonInput = new System.Windows.Forms.RadioButton();
            this.radioButtonOutput = new System.Windows.Forms.RadioButton();
            this.radioButtonInout = new System.Windows.Forms.RadioButton();
            this.radioButtonBuffer = new System.Windows.Forms.RadioButton();
            this.groupBoxClock = new System.Windows.Forms.GroupBox();
            this.checkBoxClock = new System.Windows.Forms.CheckBox();
            this.checkBoxClockEnable = new System.Windows.Forms.CheckBox();
            this.groupBoxType2 = new System.Windows.Forms.GroupBox();
            this.radioButtonCombinatioral = new System.Windows.Forms.RadioButton();
            this.radioButtonRegistered = new System.Windows.Forms.RadioButton();
            this.radioButtonClocked = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBoxType.SuspendLayout();
            this.groupBoxIntegerRange.SuspendLayout();
            this.groupBoxBusRange.SuspendLayout();
            this.groupBoxPortProperties.SuspendLayout();
            this.groupBoxClock.SuspendLayout();
            this.groupBoxType2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name: ";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(12, 29);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(100, 20);
            this.textBoxName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Value:";
            // 
            // textBoxValue
            // 
            this.textBoxValue.Location = new System.Drawing.Point(12, 68);
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.Size = new System.Drawing.Size(100, 20);
            this.textBoxValue.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBoxType);
            this.groupBox1.Controls.Add(this.groupBoxIntegerRange);
            this.groupBox1.Controls.Add(this.groupBoxBusRange);
            this.groupBox1.Controls.Add(this.radioButtonUserDefined);
            this.groupBox1.Controls.Add(this.radioButtonCharacter);
            this.groupBox1.Controls.Add(this.radioButtonBoolean);
            this.groupBox1.Controls.Add(this.radioButtonInteger);
            this.groupBox1.Controls.Add(this.radioButtonBit);
            this.groupBox1.Controls.Add(this.radioButtonStd_uLogic);
            this.groupBox1.Controls.Add(this.radioButtonStd_Logic);
            this.groupBox1.Location = new System.Drawing.Point(117, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(526, 317);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Type";
            // 
            // groupBoxType
            // 
            this.groupBoxType.Controls.Add(this.comboBoxType);
            this.groupBoxType.Location = new System.Drawing.Point(101, 265);
            this.groupBoxType.Name = "groupBoxType";
            this.groupBoxType.Size = new System.Drawing.Size(414, 46);
            this.groupBoxType.TabIndex = 9;
            this.groupBoxType.TabStop = false;
            this.groupBoxType.Text = "Type/Initial value";
            // 
            // comboBoxType
            // 
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] {
            "amy type:= initial value",
            "STD_LOgIC := 0",
            "STD_LOGIC_VECTOR (0 to 7) := \"00000000\"",
            "STD_LOGIC_VECTOR (7 downto 0) := (others => \'0\')",
            "STD_ULOGIC := \'0\'",
            "STD_ULOGIC_VECTOR (0 to 7) := \"00000000\"",
            "STD_ULOGIC_VECTOR (7 downto 0) := (others => \'0\')",
            "BIT := \'0\'",
            "BIt_VECTOR (0 to 7) := \"00000000\"",
            "BIT_VECTOR (7 downto 0) := (others => \'0\')",
            "INTEGER := 0",
            "INTEGER range 0 to 127 := 0",
            "BOOLEAN := false",
            "CHARACTER := \'A\'"});
            this.comboBoxType.Location = new System.Drawing.Point(7, 17);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(400, 21);
            this.comboBoxType.TabIndex = 0;
            // 
            // groupBoxIntegerRange
            // 
            this.groupBoxIntegerRange.Controls.Add(this.comboBoxIntegerRange2);
            this.groupBoxIntegerRange.Controls.Add(this.comboBoxIntegerRange1);
            this.groupBoxIntegerRange.Controls.Add(this.checkBoxIntegerAvalable);
            this.groupBoxIntegerRange.Controls.Add(this.radioButtonIntegerDownTo);
            this.groupBoxIntegerRange.Controls.Add(this.radioButtonIntegerTo);
            this.groupBoxIntegerRange.Location = new System.Drawing.Point(101, 126);
            this.groupBoxIntegerRange.Name = "groupBoxIntegerRange";
            this.groupBoxIntegerRange.Size = new System.Drawing.Size(414, 72);
            this.groupBoxIntegerRange.TabIndex = 8;
            this.groupBoxIntegerRange.TabStop = false;
            this.groupBoxIntegerRange.Text = "Integer Range";
            // 
            // comboBoxIntegerRange2
            // 
            this.comboBoxIntegerRange2.FormattingEnabled = true;
            this.comboBoxIntegerRange2.Location = new System.Drawing.Point(287, 14);
            this.comboBoxIntegerRange2.Name = "comboBoxIntegerRange2";
            this.comboBoxIntegerRange2.Size = new System.Drawing.Size(119, 21);
            this.comboBoxIntegerRange2.TabIndex = 8;
            // 
            // comboBoxIntegerRange1
            // 
            this.comboBoxIntegerRange1.FormattingEnabled = true;
            this.comboBoxIntegerRange1.Location = new System.Drawing.Point(7, 15);
            this.comboBoxIntegerRange1.Name = "comboBoxIntegerRange1";
            this.comboBoxIntegerRange1.Size = new System.Drawing.Size(119, 21);
            this.comboBoxIntegerRange1.TabIndex = 7;
            // 
            // checkBoxIntegerAvalable
            // 
            this.checkBoxIntegerAvalable.AutoSize = true;
            this.checkBoxIntegerAvalable.Location = new System.Drawing.Point(132, 49);
            this.checkBoxIntegerAvalable.Name = "checkBoxIntegerAvalable";
            this.checkBoxIntegerAvalable.Size = new System.Drawing.Size(69, 17);
            this.checkBoxIntegerAvalable.TabIndex = 4;
            this.checkBoxIntegerAvalable.Text = "Avaliable";
            this.checkBoxIntegerAvalable.UseVisualStyleBackColor = true;
            // 
            // radioButtonIntegerDownTo
            // 
            this.radioButtonIntegerDownTo.AutoSize = true;
            this.radioButtonIntegerDownTo.Location = new System.Drawing.Point(212, 15);
            this.radioButtonIntegerDownTo.Name = "radioButtonIntegerDownTo";
            this.radioButtonIntegerDownTo.Size = new System.Drawing.Size(69, 17);
            this.radioButtonIntegerDownTo.TabIndex = 3;
            this.radioButtonIntegerDownTo.Text = "Down To";
            this.radioButtonIntegerDownTo.UseVisualStyleBackColor = true;
            // 
            // radioButtonIntegerTo
            // 
            this.radioButtonIntegerTo.AutoSize = true;
            this.radioButtonIntegerTo.Checked = true;
            this.radioButtonIntegerTo.Location = new System.Drawing.Point(132, 15);
            this.radioButtonIntegerTo.Name = "radioButtonIntegerTo";
            this.radioButtonIntegerTo.Size = new System.Drawing.Size(38, 17);
            this.radioButtonIntegerTo.TabIndex = 2;
            this.radioButtonIntegerTo.TabStop = true;
            this.radioButtonIntegerTo.Text = "To";
            this.radioButtonIntegerTo.UseVisualStyleBackColor = true;
            // 
            // groupBoxBusRange
            // 
            this.groupBoxBusRange.Controls.Add(this.comboBoxBusRange2);
            this.groupBoxBusRange.Controls.Add(this.comboBoxBusRange1);
            this.groupBoxBusRange.Controls.Add(this.checkBoxBusAvaliable);
            this.groupBoxBusRange.Controls.Add(this.radioButtonBusDownTo);
            this.groupBoxBusRange.Controls.Add(this.radioButtonBusTo);
            this.groupBoxBusRange.Location = new System.Drawing.Point(101, 16);
            this.groupBoxBusRange.Name = "groupBoxBusRange";
            this.groupBoxBusRange.Size = new System.Drawing.Size(414, 69);
            this.groupBoxBusRange.TabIndex = 7;
            this.groupBoxBusRange.TabStop = false;
            this.groupBoxBusRange.Text = "Bus Range";
            // 
            // comboBoxBusRange2
            // 
            this.comboBoxBusRange2.FormattingEnabled = true;
            this.comboBoxBusRange2.Location = new System.Drawing.Point(287, 14);
            this.comboBoxBusRange2.Name = "comboBoxBusRange2";
            this.comboBoxBusRange2.Size = new System.Drawing.Size(119, 21);
            this.comboBoxBusRange2.TabIndex = 6;
            // 
            // comboBoxBusRange1
            // 
            this.comboBoxBusRange1.FormattingEnabled = true;
            this.comboBoxBusRange1.Location = new System.Drawing.Point(7, 15);
            this.comboBoxBusRange1.Name = "comboBoxBusRange1";
            this.comboBoxBusRange1.Size = new System.Drawing.Size(119, 21);
            this.comboBoxBusRange1.TabIndex = 5;
            // 
            // checkBoxBusAvaliable
            // 
            this.checkBoxBusAvaliable.AutoSize = true;
            this.checkBoxBusAvaliable.Location = new System.Drawing.Point(132, 46);
            this.checkBoxBusAvaliable.Name = "checkBoxBusAvaliable";
            this.checkBoxBusAvaliable.Size = new System.Drawing.Size(69, 17);
            this.checkBoxBusAvaliable.TabIndex = 4;
            this.checkBoxBusAvaliable.Text = "Avaliable";
            this.checkBoxBusAvaliable.UseVisualStyleBackColor = true;
            // 
            // radioButtonBusDownTo
            // 
            this.radioButtonBusDownTo.AutoSize = true;
            this.radioButtonBusDownTo.Location = new System.Drawing.Point(212, 15);
            this.radioButtonBusDownTo.Name = "radioButtonBusDownTo";
            this.radioButtonBusDownTo.Size = new System.Drawing.Size(69, 17);
            this.radioButtonBusDownTo.TabIndex = 3;
            this.radioButtonBusDownTo.Text = "Down To";
            this.radioButtonBusDownTo.UseVisualStyleBackColor = true;
            // 
            // radioButtonBusTo
            // 
            this.radioButtonBusTo.AutoSize = true;
            this.radioButtonBusTo.Checked = true;
            this.radioButtonBusTo.Location = new System.Drawing.Point(132, 15);
            this.radioButtonBusTo.Name = "radioButtonBusTo";
            this.radioButtonBusTo.Size = new System.Drawing.Size(38, 17);
            this.radioButtonBusTo.TabIndex = 2;
            this.radioButtonBusTo.TabStop = true;
            this.radioButtonBusTo.Text = "To";
            this.radioButtonBusTo.UseVisualStyleBackColor = true;
            // 
            // radioButtonUserDefined
            // 
            this.radioButtonUserDefined.AutoSize = true;
            this.radioButtonUserDefined.Location = new System.Drawing.Point(6, 283);
            this.radioButtonUserDefined.Name = "radioButtonUserDefined";
            this.radioButtonUserDefined.Size = new System.Drawing.Size(87, 17);
            this.radioButtonUserDefined.TabIndex = 6;
            this.radioButtonUserDefined.TabStop = true;
            this.radioButtonUserDefined.Text = "User Defined";
            this.radioButtonUserDefined.UseVisualStyleBackColor = true;
            this.radioButtonUserDefined.CheckedChanged += new System.EventHandler(this.radioButtonUserDefined_CheckedChanged);
            // 
            // radioButtonCharacter
            // 
            this.radioButtonCharacter.AutoSize = true;
            this.radioButtonCharacter.Location = new System.Drawing.Point(6, 226);
            this.radioButtonCharacter.Name = "radioButtonCharacter";
            this.radioButtonCharacter.Size = new System.Drawing.Size(71, 17);
            this.radioButtonCharacter.TabIndex = 5;
            this.radioButtonCharacter.TabStop = true;
            this.radioButtonCharacter.Text = "Character";
            this.radioButtonCharacter.UseVisualStyleBackColor = true;
            this.radioButtonCharacter.CheckedChanged += new System.EventHandler(this.radioButtonBoolean_CheckedChanged);
            // 
            // radioButtonBoolean
            // 
            this.radioButtonBoolean.AutoSize = true;
            this.radioButtonBoolean.Location = new System.Drawing.Point(6, 202);
            this.radioButtonBoolean.Name = "radioButtonBoolean";
            this.radioButtonBoolean.Size = new System.Drawing.Size(64, 17);
            this.radioButtonBoolean.TabIndex = 4;
            this.radioButtonBoolean.TabStop = true;
            this.radioButtonBoolean.Text = "Boolean";
            this.radioButtonBoolean.UseVisualStyleBackColor = true;
            this.radioButtonBoolean.CheckedChanged += new System.EventHandler(this.radioButtonBoolean_CheckedChanged);
            // 
            // radioButtonInteger
            // 
            this.radioButtonInteger.AutoSize = true;
            this.radioButtonInteger.Location = new System.Drawing.Point(7, 147);
            this.radioButtonInteger.Name = "radioButtonInteger";
            this.radioButtonInteger.Size = new System.Drawing.Size(58, 17);
            this.radioButtonInteger.TabIndex = 3;
            this.radioButtonInteger.TabStop = true;
            this.radioButtonInteger.Text = "Integer";
            this.radioButtonInteger.UseVisualStyleBackColor = true;
            this.radioButtonInteger.CheckedChanged += new System.EventHandler(this.radioButtonInteger_CheckedChanged);
            // 
            // radioButtonBit
            // 
            this.radioButtonBit.AutoSize = true;
            this.radioButtonBit.Location = new System.Drawing.Point(7, 68);
            this.radioButtonBit.Name = "radioButtonBit";
            this.radioButtonBit.Size = new System.Drawing.Size(37, 17);
            this.radioButtonBit.TabIndex = 2;
            this.radioButtonBit.TabStop = true;
            this.radioButtonBit.Text = "Bit";
            this.radioButtonBit.UseVisualStyleBackColor = true;
            this.radioButtonBit.CheckedChanged += new System.EventHandler(this.radioButtonStd_Logic_CheckedChanged);
            // 
            // radioButtonStd_uLogic
            // 
            this.radioButtonStd_uLogic.AutoSize = true;
            this.radioButtonStd_uLogic.Location = new System.Drawing.Point(7, 44);
            this.radioButtonStd_uLogic.Name = "radioButtonStd_uLogic";
            this.radioButtonStd_uLogic.Size = new System.Drawing.Size(79, 17);
            this.radioButtonStd_uLogic.TabIndex = 1;
            this.radioButtonStd_uLogic.TabStop = true;
            this.radioButtonStd_uLogic.Text = "Std_uLogic";
            this.radioButtonStd_uLogic.UseVisualStyleBackColor = true;
            this.radioButtonStd_uLogic.CheckedChanged += new System.EventHandler(this.radioButtonStd_Logic_CheckedChanged);
            // 
            // radioButtonStd_Logic
            // 
            this.radioButtonStd_Logic.AutoSize = true;
            this.radioButtonStd_Logic.Location = new System.Drawing.Point(7, 20);
            this.radioButtonStd_Logic.Name = "radioButtonStd_Logic";
            this.radioButtonStd_Logic.Size = new System.Drawing.Size(73, 17);
            this.radioButtonStd_Logic.TabIndex = 0;
            this.radioButtonStd_Logic.TabStop = true;
            this.radioButtonStd_Logic.Text = "Std_Logic";
            this.radioButtonStd_Logic.UseVisualStyleBackColor = true;
            this.radioButtonStd_Logic.CheckedChanged += new System.EventHandler(this.radioButtonStd_Logic_CheckedChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(12, 307);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(12, 278);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // comboBoxGenType
            // 
            this.comboBoxGenType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGenType.FormattingEnabled = true;
            this.comboBoxGenType.Location = new System.Drawing.Point(0, 9);
            this.comboBoxGenType.Name = "comboBoxGenType";
            this.comboBoxGenType.Size = new System.Drawing.Size(100, 21);
            this.comboBoxGenType.TabIndex = 7;
            // 
            // groupBoxPortProperties
            // 
            this.groupBoxPortProperties.Controls.Add(this.groupBoxType2);
            this.groupBoxPortProperties.Controls.Add(this.groupBoxClock);
            this.groupBoxPortProperties.Controls.Add(this.radioButtonBuffer);
            this.groupBoxPortProperties.Controls.Add(this.radioButtonInout);
            this.groupBoxPortProperties.Controls.Add(this.radioButtonOutput);
            this.groupBoxPortProperties.Controls.Add(this.radioButtonInput);
            this.groupBoxPortProperties.Controls.Add(this.comboBoxGenType);
            this.groupBoxPortProperties.Location = new System.Drawing.Point(12, 95);
            this.groupBoxPortProperties.Name = "groupBoxPortProperties";
            this.groupBoxPortProperties.Size = new System.Drawing.Size(206, 175);
            this.groupBoxPortProperties.TabIndex = 8;
            this.groupBoxPortProperties.TabStop = false;
            this.groupBoxPortProperties.Text = "Port Properties";
            // 
            // radioButtonInput
            // 
            this.radioButtonInput.AutoSize = true;
            this.radioButtonInput.Location = new System.Drawing.Point(7, 20);
            this.radioButtonInput.Name = "radioButtonInput";
            this.radioButtonInput.Size = new System.Drawing.Size(49, 17);
            this.radioButtonInput.TabIndex = 0;
            this.radioButtonInput.TabStop = true;
            this.radioButtonInput.Text = "Input";
            this.radioButtonInput.UseVisualStyleBackColor = true;
            this.radioButtonInput.CheckedChanged += new System.EventHandler(this.radioButtonInput_CheckedChanged);
            // 
            // radioButtonOutput
            // 
            this.radioButtonOutput.AutoSize = true;
            this.radioButtonOutput.Location = new System.Drawing.Point(7, 98);
            this.radioButtonOutput.Name = "radioButtonOutput";
            this.radioButtonOutput.Size = new System.Drawing.Size(57, 17);
            this.radioButtonOutput.TabIndex = 1;
            this.radioButtonOutput.TabStop = true;
            this.radioButtonOutput.Text = "Output";
            this.radioButtonOutput.UseVisualStyleBackColor = true;
            this.radioButtonOutput.CheckedChanged += new System.EventHandler(this.radioButtonOutput_CheckedChanged);
            // 
            // radioButtonInout
            // 
            this.radioButtonInout.AutoSize = true;
            this.radioButtonInout.Location = new System.Drawing.Point(7, 122);
            this.radioButtonInout.Name = "radioButtonInout";
            this.radioButtonInout.Size = new System.Drawing.Size(49, 17);
            this.radioButtonInout.TabIndex = 2;
            this.radioButtonInout.TabStop = true;
            this.radioButtonInout.Text = "Inout";
            this.radioButtonInout.UseVisualStyleBackColor = true;
            this.radioButtonInout.CheckedChanged += new System.EventHandler(this.radioButtonOutput_CheckedChanged);
            // 
            // radioButtonBuffer
            // 
            this.radioButtonBuffer.AutoSize = true;
            this.radioButtonBuffer.Location = new System.Drawing.Point(7, 146);
            this.radioButtonBuffer.Name = "radioButtonBuffer";
            this.radioButtonBuffer.Size = new System.Drawing.Size(53, 17);
            this.radioButtonBuffer.TabIndex = 3;
            this.radioButtonBuffer.TabStop = true;
            this.radioButtonBuffer.Text = "Buffer";
            this.radioButtonBuffer.UseVisualStyleBackColor = true;
            this.radioButtonBuffer.CheckedChanged += new System.EventHandler(this.radioButtonOutput_CheckedChanged);
            // 
            // groupBoxClock
            // 
            this.groupBoxClock.Controls.Add(this.checkBoxClockEnable);
            this.groupBoxClock.Controls.Add(this.checkBoxClock);
            this.groupBoxClock.Location = new System.Drawing.Point(79, 9);
            this.groupBoxClock.Name = "groupBoxClock";
            this.groupBoxClock.Size = new System.Drawing.Size(100, 66);
            this.groupBoxClock.TabIndex = 4;
            this.groupBoxClock.TabStop = false;
            this.groupBoxClock.Text = "Clock";
            // 
            // checkBoxClock
            // 
            this.checkBoxClock.AutoSize = true;
            this.checkBoxClock.Location = new System.Drawing.Point(7, 15);
            this.checkBoxClock.Name = "checkBoxClock";
            this.checkBoxClock.Size = new System.Drawing.Size(53, 17);
            this.checkBoxClock.TabIndex = 0;
            this.checkBoxClock.Text = "Clock";
            this.checkBoxClock.UseVisualStyleBackColor = true;
            this.checkBoxClock.CheckedChanged += new System.EventHandler(this.checkBoxClock_CheckedChanged);
            // 
            // checkBoxClockEnable
            // 
            this.checkBoxClockEnable.AutoSize = true;
            this.checkBoxClockEnable.Location = new System.Drawing.Point(7, 39);
            this.checkBoxClockEnable.Name = "checkBoxClockEnable";
            this.checkBoxClockEnable.Size = new System.Drawing.Size(89, 17);
            this.checkBoxClockEnable.TabIndex = 1;
            this.checkBoxClockEnable.Text = "Clock Enable";
            this.checkBoxClockEnable.UseVisualStyleBackColor = true;
            this.checkBoxClockEnable.CheckedChanged += new System.EventHandler(this.checkBoxClockEnable_CheckedChanged);
            // 
            // groupBoxType2
            // 
            this.groupBoxType2.Controls.Add(this.radioButtonClocked);
            this.groupBoxType2.Controls.Add(this.radioButtonRegistered);
            this.groupBoxType2.Controls.Add(this.radioButtonCombinatioral);
            this.groupBoxType2.Location = new System.Drawing.Point(79, 82);
            this.groupBoxType2.Name = "groupBoxType2";
            this.groupBoxType2.Size = new System.Drawing.Size(121, 87);
            this.groupBoxType2.TabIndex = 5;
            this.groupBoxType2.TabStop = false;
            this.groupBoxType2.Text = "Type";
            // 
            // radioButtonCombinatioral
            // 
            this.radioButtonCombinatioral.AutoSize = true;
            this.radioButtonCombinatioral.Location = new System.Drawing.Point(7, 16);
            this.radioButtonCombinatioral.Name = "radioButtonCombinatioral";
            this.radioButtonCombinatioral.Size = new System.Drawing.Size(88, 17);
            this.radioButtonCombinatioral.TabIndex = 0;
            this.radioButtonCombinatioral.TabStop = true;
            this.radioButtonCombinatioral.Text = "Combinatioral";
            this.radioButtonCombinatioral.UseVisualStyleBackColor = true;
            // 
            // radioButtonRegistered
            // 
            this.radioButtonRegistered.AutoSize = true;
            this.radioButtonRegistered.Location = new System.Drawing.Point(7, 40);
            this.radioButtonRegistered.Name = "radioButtonRegistered";
            this.radioButtonRegistered.Size = new System.Drawing.Size(76, 17);
            this.radioButtonRegistered.TabIndex = 1;
            this.radioButtonRegistered.TabStop = true;
            this.radioButtonRegistered.Text = "Registered";
            this.radioButtonRegistered.UseVisualStyleBackColor = true;
            // 
            // radioButtonClocked
            // 
            this.radioButtonClocked.AutoSize = true;
            this.radioButtonClocked.Location = new System.Drawing.Point(7, 64);
            this.radioButtonClocked.Name = "radioButtonClocked";
            this.radioButtonClocked.Size = new System.Drawing.Size(64, 17);
            this.radioButtonClocked.TabIndex = 2;
            this.radioButtonClocked.TabStop = true;
            this.radioButtonClocked.Text = "Clocked";
            this.radioButtonClocked.UseVisualStyleBackColor = true;
            // 
            // SignalProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 342);
            this.Controls.Add(this.groupBoxPortProperties);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBoxValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SignalProperties";
            this.Text = "Signal Properties";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxType.ResumeLayout(false);
            this.groupBoxIntegerRange.ResumeLayout(false);
            this.groupBoxIntegerRange.PerformLayout();
            this.groupBoxBusRange.ResumeLayout(false);
            this.groupBoxBusRange.PerformLayout();
            this.groupBoxPortProperties.ResumeLayout(false);
            this.groupBoxPortProperties.PerformLayout();
            this.groupBoxClock.ResumeLayout(false);
            this.groupBoxClock.PerformLayout();
            this.groupBoxType2.ResumeLayout(false);
            this.groupBoxType2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxValue;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.RadioButton radioButtonUserDefined;
        private System.Windows.Forms.RadioButton radioButtonCharacter;
        private System.Windows.Forms.RadioButton radioButtonBoolean;
        private System.Windows.Forms.RadioButton radioButtonInteger;
        private System.Windows.Forms.RadioButton radioButtonBit;
        private System.Windows.Forms.RadioButton radioButtonStd_uLogic;
        private System.Windows.Forms.RadioButton radioButtonStd_Logic;
        private System.Windows.Forms.GroupBox groupBoxBusRange;
        private System.Windows.Forms.RadioButton radioButtonBusDownTo;
        private System.Windows.Forms.RadioButton radioButtonBusTo;
        private System.Windows.Forms.GroupBox groupBoxIntegerRange;
        private System.Windows.Forms.RadioButton radioButtonIntegerDownTo;
        private System.Windows.Forms.RadioButton radioButtonIntegerTo;
        private System.Windows.Forms.GroupBox groupBoxType;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.ComboBox comboBoxGenType;
        private System.Windows.Forms.CheckBox checkBoxIntegerAvalable;
        private System.Windows.Forms.ComboBox comboBoxIntegerRange2;
        private System.Windows.Forms.ComboBox comboBoxIntegerRange1;
        private System.Windows.Forms.ComboBox comboBoxBusRange2;
        private System.Windows.Forms.ComboBox comboBoxBusRange1;
        private System.Windows.Forms.CheckBox checkBoxBusAvaliable;
        private System.Windows.Forms.GroupBox groupBoxPortProperties;
        private System.Windows.Forms.RadioButton radioButtonBuffer;
        private System.Windows.Forms.RadioButton radioButtonInout;
        private System.Windows.Forms.RadioButton radioButtonOutput;
        private System.Windows.Forms.RadioButton radioButtonInput;
        private System.Windows.Forms.GroupBox groupBoxClock;
        private System.Windows.Forms.CheckBox checkBoxClock;
        private System.Windows.Forms.CheckBox checkBoxClockEnable;
        private System.Windows.Forms.GroupBox groupBoxType2;
        private System.Windows.Forms.RadioButton radioButtonClocked;
        private System.Windows.Forms.RadioButton radioButtonRegistered;
        private System.Windows.Forms.RadioButton radioButtonCombinatioral;
    }
}