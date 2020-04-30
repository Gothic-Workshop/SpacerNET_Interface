﻿
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SpacerUnion
{
    public partial class ObjTree : Form
    {
        #region Imports


        [DllImport("SpacerUnionNet.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Extern_SaveVobTree(IntPtr str);

        [DllImport("SpacerUnionNet.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Extern_OpenVobTree(IntPtr str, bool globalInsert);

        [DllImport("SpacerUnionNet.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Extern_SelectVobSync(uint a);

        [DllImport("SpacerUnionNet.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Extern_SelectVob(uint a);

        [DllImport("SpacerUnionNet.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Extern_BlockMouse(bool disable);

        [DllImport("SpacerUnionNet.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Extern_RemoveVob(uint vob);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        [DllImport("SpacerUnionNet.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Extern_GetSettingStr(IntPtr namePtr);


        [DllImport("SpacerUnionNet.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Extern_SetSettingStr(IntPtr namePtr, IntPtr optionName);

        #endregion

        public static TreeNode previousSelectedNode = null;


        const string TAG_FOLDER = "folder";

        public static Dictionary<uint, TreeEntry> globalEntries = new Dictionary<uint, TreeEntry>();
        static bool nextAfterEventBlocked = false;
        static TreeNode lastSelectedNode = null;

        public ObjTree()
        {
            InitializeComponent();

        }

        

  
        public static int CreateAndGetFolder(string className)
        {
            TreeNodeCollection nodes = UnionNET.objTreeWin.globalTree.Nodes;

            int classNameFoundPos = -1;

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Text == className)
                {
                    classNameFoundPos = i;
                    break;
                }
            }

            if (classNameFoundPos == -1)
            {

                TreeNode newNode = nodes.Add(className);
                newNode.Tag = TAG_FOLDER;
                newNode.ImageIndex = 0;
                newNode.SelectedImageIndex = 0;

                classNameFoundPos = newNode.Index;
            }

            return classNameFoundPos;
        }


        public static int noParentCount = 0;

        public static void ApplyNodeImage(string className, TreeNode node, bool isNew=false)
        {
            if (isNew)
            {
                node.ImageIndex = 3;
                node.SelectedImageIndex = 3;
                return;
            }

            if (className == "zCVobWaypoint" || className == "zCVobSpot")
            {
                node.ImageIndex = 1;
                node.SelectedImageIndex = 1;
            }
            else
            {
                node.ImageIndex = 1;
                node.SelectedImageIndex = 1;
            }

            if (node.Tag.ToString() == TAG_FOLDER)
            {
                node.ImageIndex = 0;
                node.SelectedImageIndex = 0;
            }

        }

        public static void AddVobToNodes(TreeEntry entry)
        {
            TreeNodeCollection nodes = UnionNET.objTreeWin.globalTree.Nodes;

            string className = entry.className;

            int classNameFoundPos = -1;

            classNameFoundPos = CreateAndGetFolder(className);

            // levelCompo или воб без родителя
            if (entry.isLevel || entry.parent == 0)
            {
                TreeNode node = nodes[classNameFoundPos].Nodes.Add(entry.name);
                node.Tag = entry.zCVob;
                
                entry.node = node;

                ApplyNodeImage(className, node);


            }
            else if (entry.parentEntry != null)
            {
                if (!entry.parentEntry.isLevel)
                {
                    TreeNode parentNode = entry.parentEntry.node;

                    if (parentNode == null)
                    {
                        noParentCount++;


                        ConsoleEx.WriteLineRed(noParentCount + " ParentNode " + Utils.ToHex(entry.parent) + " is null: " + entry.name);
                        return;
                    }


                    string name = entry.name; //entry.zCVob + " " + entry.name + "[" + entry.parent + "]"
                                              //Console.WriteLine("Parent ok");

                    TreeNode node = parentNode.Nodes.Add(name);
                    node.Tag = entry.zCVob;
                    node.ImageIndex = 1;
                    node.SelectedImageIndex = 1;
                    entry.node = node;
                    ApplyNodeImage(className, node);
                }
                else
                {
                    string name = entry.name; //"!!! : " + entry.zCVob + " " + entry.name + "[" + entry.parent + "]"


                    TreeNode node = nodes[classNameFoundPos].Nodes.Add(name);
                    node.Tag = entry.zCVob;
                    node.ImageIndex = 1;
                    node.SelectedImageIndex = 1;
                    entry.node = node;
                    ApplyNodeImage(className, node);

                }

            }
            else
            {
                noParentCount++;


                ConsoleEx.WriteLineRed(noParentCount + " ParentEntry " + Utils.ToHex(entry.parent) + " is null: " + entry.name);


                Utils.WriteToFile(String.Format("AddVobToNodes: Parent entry is null, parent {0}, name {1}", entry.parent, entry.name));
            }

        }


        

        
        [DllExport]
        public static void UpdateVobName(uint ptr, IntPtr namePtr)
        {
            if (ptr == 0)
            {
                return;
            }

            Stopwatch s = new Stopwatch();
            s.Start();

            string name = Marshal.PtrToStringAnsi(namePtr);

            TreeEntry entry = null;

            try
            {
                entry = globalEntries
                    .Where(x => x.Value.zCVob == ptr)
                    .Select(pair => pair.Value)
                    .FirstOrDefault();
            }
            catch
            {
                ConsoleEx.WriteLineRed("C#: UpdateName fail. No vob found in globalList. Addr: " + Utils.ToHex(ptr));

                Utils.WriteToFile(String.Format("UpdateName: No vob found in globalList addr: {0}, name {1}", Utils.ToHex(ptr), name));
            }


            if (entry != null)
            {
                if (entry.node != null)
                {
                    entry.name = name;
                    entry.node.Text = name;
                    UnionNET.objTreeWin.globalTree.SelectedNode = entry.node;
                }
            }


            Console.WriteLine("C#: UpdateName for vob: " + Utils.ToHex(ptr));


        }

        [DllExport]
        public static void OnSelectVob(uint ptr)
        {

            if (ptr == 0)
            {
                return;
            }

            TreeEntry entry = null;


            entry = globalEntries
                    .Where(x => x.Value.zCVob == ptr)
                    .Select(pair => pair.Value)
                    .FirstOrDefault();


            Console.WriteLine("C#: OnSelectVob: " + Utils.ToHex(ptr));


            if (entry != null)
            {
                if (entry.node != null)
                {
                    UnionNET.objTreeWin.globalTree.SelectedNode = entry.node;
                }
                else
                {
                    Utils.Error("OnSelectVob: entry.node is null, key/addr/vob is " + Utils.ToHex(ptr));
                }
               
            }
            else
            {
                Utils.Error("OnSelectVob: No key/addr/vob found in globalList. Key: " + Utils.ToHex(ptr));
            }
        }


        public static void RemoveChildNodesRecursive(TreeEntry entry)
        {
            for (int i = 0; i < entry.childs.Count; i++)
            {
                RemoveChildNodesRecursive(entry.childs[i]);
            }

            if (entry.node != null)
            {
                Console.WriteLine("C#: Remove node: " + entry.node.Text + " Parent: " + Utils.ToHex(entry.parent));
                entry.node.Remove();
            }
            else
            {
                Console.WriteLine("C#: Remove node failure. Node is null");
            }

            entry.childs.Clear();
            entry.toDelete = true;
            entry.node = null;
        }

        [DllExport]
        public static void OnVobRemove(uint vob)
        {
            TreeNodeCollection nodes = UnionNET.objTreeWin.globalTree.Nodes;

            Console.WriteLine("=============================");
            Console.WriteLine("C#: OnVobRemove: " + Utils.ToHex(vob));
            ConsoleEx.WriteLineGreen("C#: Всего вобов в списке: " + globalEntries.Count);

            if (vob == 0)
            {
                return;
            }


            List<TreeEntry> entries = globalEntries
                    .Where(pair => pair.Value.zCVob == vob)
                    .Select(pair => pair.Value)
                    .ToList();


            //Console.WriteLine("Found entries with Vob value: " + entries.Count);

            if (entries.Count > 0)
            {
                TreeEntry entry = entries[0];

                RemoveChildNodesRecursive(entry);

                globalEntries = globalEntries
                        .Where(pair => !pair.Value.toDelete)
                        .ToDictionary(pair=>pair.Key, pair=>pair.Value);

                if (entries.Count > 1)
                {
                    ConsoleEx.WriteLineRed("C#: Warning! I found more than 1 entries with same Vob addr! Count: " + entries.Count);


                    Utils.WriteToFile(String.Format("Warning! I found more than 1 entries with same Vob addr! addr: {0}, count {1}", vob, entries.Count));
                }
            }

            UnionNET.vobList.ClearListBox();

            ConsoleEx.WriteLineGreen("C#: Всего вобов в списке: " + globalEntries.Count);
            Console.WriteLine("=============================");
        }

        [DllExport]
        public static void OnDynamicUpdateVob(IntPtr ptr, uint vob, uint parent, IntPtr classNamePtr)
        {
            string name = Marshal.PtrToStringAnsi(ptr);
            string className = Marshal.PtrToStringAnsi(classNamePtr);

            TreeNodeCollection nodes = UnionNET.objTreeWin.globalTree.Nodes;

            Console.WriteLine("=============================");

            int classNameFoundPos = -1;

            classNameFoundPos = CreateAndGetFolder(className);

            TreeEntry entry = new TreeEntry();

            entry.name = name;
            entry.parent = parent;
            entry.zCVob = vob;
            entry.className = className;
            entry.isLevel = entry.className == "zCVobLevelCompo";

            if (!globalEntries.ContainsKey(vob))
            {
                globalEntries.Add(vob, entry);
            }
            else
            {

                string msg = "==============================\nОшибка! Пытаюсь добавить воб "
                    + Utils.ToHex(vob)
                    + ", но такой адрес-ключ уже есть в globalEntries!"
                    + "\nNewVob: " + globalEntries[vob].name
                    + " addr: zCVob = "
                    + Utils.ToHex(globalEntries[vob].zCVob)
                    + ", Parent: " + Utils.ToHex(globalEntries[vob].parent)
                    + ", ChildrenCount: " + globalEntries[vob].childs.Count
                    + ", ToDelete: " + globalEntries[vob].toDelete;

                if (globalEntries[vob].node != null)
                {
                    msg += "\nnode.text: " + globalEntries[vob].node.Text;
                    msg += ", node.Tag: " + Utils.ToHex(Convert.ToUInt32(globalEntries[vob].node.Tag.ToString()));
                }

                if (globalEntries[vob].parentEntry != null)
                {
                    msg += "\nparentEntryName: " + globalEntries[vob].parentEntry.name;
                    msg += ", parentEntryClassName: " + globalEntries[vob].parentEntry.className;
                    msg += ", parentEntryAddr: " + Utils.ToHex(globalEntries[vob].parentEntry.zCVob);
                }

                msg += "\n==============================";

                Utils.WriteToFile(msg);
                ConsoleEx.WriteLineRed(msg);

                MessageBox.Show(msg);
                return;
            }




            TreeEntry foundEntry = null;

            try
            {
                foundEntry = globalEntries[entry.parent];
            }
            catch
            {
                foundEntry = null;

                //ConsoleEx.WriteLineRed("C#: OnVobInsert. Can't found parent entry!");
                //Utils.WriteToFile("C#: OnVobInsert. Can't found parent entry!");
            }

            if (foundEntry != null)
            {
                entry.parentEntry = foundEntry;
                foundEntry.childs.Add(entry);
            }

            nextAfterEventBlocked = true;
            lastSelectedNode = UnionNET.objTreeWin.globalTree.SelectedNode;

            if (parent == 0)
            {
                TreeNode node = nodes[classNameFoundPos].Nodes.Add(name);
                node.Tag = vob;
                entry.node = node;
                ApplyNodeImage(className, node, true);
                UnionNET.objTreeWin.globalTree.SelectedNode = node;
                ConsoleEx.WriteLineGreen("C# OnDynamicUpdate: Insert globally: " + name + " parent: " + Utils.ToHex(parent) + " className: " + className);
            }
            else if (entry.parentEntry != null)
            {
                if (entry.parentEntry.node != null)
                {
                    TreeNode node = null;

                    if (entry.parentEntry.isLevel)
                    {
                        node = nodes[classNameFoundPos].Nodes.Add(name);
                    }
                    else
                    {
                        node = entry.parentEntry.node.Nodes.Add(name);
                    }

                    node.Tag = vob;
                    entry.node = node;
                    ApplyNodeImage(className, node, true);
                    //UnionNET.objTreeWin.globalTree.SelectedNode = node;
                }
                else
                {

                    string msg = "C# OnDynamicUpdate: parent node is null. Vob  "
                    + entry.parentEntry.name;


                    ConsoleEx.WriteLineRed(msg);
                    Utils.WriteToFile(msg);
                }

            }
            else
            {

                ConsoleEx.WriteLineRed("C# OnDynamicUpdate: parent entry is null");
                Utils.WriteToFile("C# OnDynamicUpdate: parent node is null");
            }


            ConsoleEx.WriteLineGreen("C#: Всего вобов в списке: " + globalEntries.Count);
            Console.WriteLine("=============================");
        }
        [DllExport]
        public static void OnVobInsert(IntPtr ptr, uint vob, uint parent, IntPtr classNamePtr)
        {
            string name = Marshal.PtrToStringAnsi(ptr);
            string className = Marshal.PtrToStringAnsi(classNamePtr);

            TreeNodeCollection nodes = UnionNET.objTreeWin.globalTree.Nodes;

            Console.WriteLine("=============================");

            int classNameFoundPos = -1;

            classNameFoundPos = CreateAndGetFolder(className);

            TreeEntry entry = new TreeEntry();

            entry.name = name;
            entry.parent = parent;
            entry.zCVob = vob;
            entry.className = className;
            entry.isLevel = entry.className == "zCVobLevelCompo";

            if (!globalEntries.ContainsKey(vob))
            {
                globalEntries.Add(vob, entry);
            }
            else
            {

                string msg = "==============================\nОшибка! Пытаюсь добавить воб "
                    + Utils.ToHex(vob)
                    + ", но такой адрес-ключ уже есть в globalEntries!"
                    + "\nNewVob: " + globalEntries[vob].name 
                    + " addr: zCVob = "
                    + Utils.ToHex(globalEntries[vob].zCVob)
                    + ", Parent: " + Utils.ToHex(globalEntries[vob].parent)
                    + ", ChildrenCount: " + globalEntries[vob].childs.Count
                    + ", ToDelete: " + globalEntries[vob].toDelete;

                if (globalEntries[vob].node != null)
                {
                    msg += "\nnode.text: " + globalEntries[vob].node.Text;
                    msg += ", node.Tag: " + Utils.ToHex(Convert.ToUInt32(globalEntries[vob].node.Tag.ToString()));
                }

                if (globalEntries[vob].parentEntry != null)
                {
                    msg += "\nparentEntryName: " + globalEntries[vob].parentEntry.name;
                    msg += ", parentEntryClassName: " + globalEntries[vob].parentEntry.className;
                    msg += ", parentEntryAddr: " + Utils.ToHex(globalEntries[vob].parentEntry.zCVob);
                }

                msg += "\n==============================";

                Utils.WriteToFile(msg);
                ConsoleEx.WriteLineRed(msg);

                MessageBox.Show(msg);
                return;
            }




            TreeEntry foundEntry = null;

            try
            {
                foundEntry = globalEntries[entry.parent];
            }
            catch
            {
                foundEntry = null;

                //ConsoleEx.WriteLineRed("C#: OnVobInsert. Can't found parent entry!");
                //Utils.WriteToFile("C#: OnVobInsert. Can't found parent entry!");
            }
            
            if (foundEntry != null)
            {
                entry.parentEntry = foundEntry;
                foundEntry.childs.Add(entry);
            }
            

            if (parent == 0)
            {
                TreeNode node = nodes[classNameFoundPos].Nodes.Add(name);
                node.Tag = vob;
                entry.node = node;
                ApplyNodeImage(className, node, true);
                UnionNET.objTreeWin.globalTree.SelectedNode = node;
                Console.WriteLine("C# OnVobInsert globally: " + name + " parent: " + Utils.ToHex(parent) + " className: " + className);
            }
            else if (entry.parentEntry != null)
            {
                if (entry.parentEntry.node != null)
                {
                    TreeNode node = null;

                    if (entry.parentEntry.isLevel)
                    {
                        node = nodes[classNameFoundPos].Nodes.Add(name);
                    }
                    else
                    {
                        node = entry.parentEntry.node.Nodes.Add(name);
                    }

                    node.Tag = vob;
                    entry.node = node;
                    ApplyNodeImage(className, node, true);
                    UnionNET.objTreeWin.globalTree.SelectedNode = node;
                }
                else
                {

                    string msg = "C# OnVobInsert: parent node is null. Vob  "
                    + entry.parentEntry.name;
                   

                    ConsoleEx.WriteLineRed(msg);
                    Utils.WriteToFile(msg);
                }

            }
            else
            {

                ConsoleEx.WriteLineRed("C# OnVobInsert: parent entry is null");
                Utils.WriteToFile("C# OnVobInsert: parent node is null");
            }


            ConsoleEx.WriteLineGreen("C#: Всего вобов в списке: " + globalEntries.Count);
            Console.WriteLine("=============================");
        }


        [DllExport]
        public static void CreateTree()
        {
            noParentCount = 0;

            UnionNET.objTreeWin.globalTree.Visible = false;
            
            foreach (var entry in globalEntries)
            {
                AddVobToNodes(entry.Value);
            }

            ConsoleEx.WriteLineGreen("C#: Дерево заполнено. Всего записей: " + globalEntries.Count);
            UnionNET.objTreeWin.globalTree.Visible = true;
            Application.DoEvents();

        }

        [DllExport]
        public static void AddGlobalEntry(IntPtr ptr, uint vob, uint parent, IntPtr classNamePtr)
        {

            string name = Marshal.PtrToStringAnsi(ptr);
            string className = Marshal.PtrToStringAnsi(classNamePtr);


            TreeNodeCollection nodes = UnionNET.objTreeWin.globalTree.Nodes;

            TreeEntry entry = new TreeEntry();

            entry.name = name;
            entry.parent = parent;

            entry.zCVob = vob;
            entry.className = className;
            entry.isLevel = entry.className == "zCVobLevelCompo";


            try
            {
                globalEntries.Add(vob, entry);
            }
            catch
            {

                Utils.Error("AddGlobalEntry: ключ уже существует!: Key: " + Utils.ToHex(vob) + ", Name: " + name + " Parent: " + Utils.ToHex(parent));
            }
            

            if (entry.parent == 0)
            {
                return;
            }

            TreeEntry foundEntry = null;

            try
            {
                foundEntry = globalEntries[entry.parent];
            }
            catch
            {
                foundEntry = null;

                Utils.Error("AddGlobalEntry: Не смог найти entry.parent!: " + Utils.ToHex(vob) + ", Name: " + name + " Parent: " + Utils.ToHex(entry.parent));

            }
           

            if (foundEntry != null)
            {
                entry.parentEntry = foundEntry;
                foundEntry.childs.Add(entry);
            }
        }


      

        private void buttonCollapse_Click(object sender, EventArgs e)
        {
            globalTree.CollapseAll();
        }

        private void buttonExpand_Click(object sender, EventArgs e)
        {
            globalTree.ExpandAll();
        }

        private void ObjTree_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }


        


        private void globalTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = globalTree.SelectedNode;

            string tag = node.Tag.ToString();

           


            if (tag.Length == 0 || tag == TAG_FOLDER)
            {
                return;
            }

            uint addr = Convert.ToUInt32(node.Tag);

            Console.WriteLine("C#: OnSelectDoubleClick node: vob " + Utils.ToHex(addr));

            Extern_SelectVob(addr);
            UnionNET.form.Focus();
        }

        private void globalTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView tree = sender as TreeView;

            if (nextAfterEventBlocked)
            {
                Console.WriteLine("C#: AfterSelect event was aborted.");
                nextAfterEventBlocked = false;

                if (lastSelectedNode != null)
                {
                    tree.SelectedNode = lastSelectedNode;
                }

                return;
            }
           
            TreeNode node = globalTree.SelectedNode;

            if (previousSelectedNode != null)
            {
                if (previousSelectedNode.Tag.ToString() == TAG_FOLDER)
                {
                    previousSelectedNode.SelectedImageIndex = 0;
                }
                else
                {
                    previousSelectedNode.SelectedImageIndex = 1;
                }
                
            }

            string tag = node.Tag.ToString();

            if (tag.Length == 0 || tag == TAG_FOLDER)
            {
                return;
            }

            node.SelectedImageIndex = 4;




            uint addr = Convert.ToUInt32(node.Tag);

            Console.WriteLine("C#: AfterSelect node: vob " + Utils.ToHex(addr));

            if (node.Text.Contains("zCVobWaypoint") || node.Text.Contains("zCVobSpot"))
            {
                UnionNET.partWin.tabControlObjects.SelectedIndex = 4;
            }

            Extern_SelectVobSync(addr);
            UnionNET.form.Focus();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (UnionNET.objTreeWin.globalTree.SelectedNode == null)
            {
                return;
            }

            TreeNode node = globalTree.SelectedNode;

            string tag = node.Tag.ToString();

            if (tag.Length == 0 || tag == TAG_FOLDER)
            {
                return;
            }


            saveFileDialogVobTree.Filter = "Zen file (*.zen)|";
            IntPtr ptrPath = Extern_GetSettingStr(Marshal.StringToHGlobalAnsi("treeVobPath"));
            string path = Marshal.PtrToStringAnsi(ptrPath);

            string fileName = UnionNET.objTreeWin.globalTree.SelectedNode.Text;

            //MessageBox.Show(path);

            if (path != "")
            {
                //MessageBox.Show(path);
                saveFileDialogVobTree.InitialDirectory = System.IO.Path.GetDirectoryName(@path + @"/");
            }
            else
            {
                saveFileDialogVobTree.InitialDirectory = @Directory.GetCurrentDirectory() + @"../_WORK/DATA/Worlds/";
            }

            saveFileDialogVobTree.RestoreDirectory = true;
            saveFileDialogVobTree.FileName = fileName + ".ZEN";

            //Extern_BlockMouse(true);

            if (saveFileDialogVobTree.ShowDialog() == DialogResult.OK)
            {

                IntPtr ptrPathSave = Marshal.StringToHGlobalAnsi(Path.GetDirectoryName(saveFileDialogVobTree.FileName));


                Extern_SetSettingStr(ptrPathSave, Marshal.StringToHGlobalAnsi("treeVobPath"));




                string filePath = saveFileDialogVobTree.FileName;
                IntPtr filePathPtr = Marshal.StringToHGlobalAnsi(filePath);
                Extern_SaveVobTree(filePathPtr);

                Marshal.FreeHGlobal(ptrPathSave);
                Marshal.FreeHGlobal(filePathPtr);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (globalTree.SelectedNode == null)
            {
                return;
            }

            TreeNode node = globalTree.SelectedNode;

            string tag = node.Tag.ToString();

            if (tag.Length == 0 || tag == TAG_FOLDER)
            {
                return;
            }


            openFileDialogVobTree.Filter = "Zen file (*.zen)|";
            IntPtr ptrPath = Extern_GetSettingStr(Marshal.StringToHGlobalAnsi("treeVobPath"));
            string path = Marshal.PtrToStringAnsi(ptrPath);

            string fileName = UnionNET.objTreeWin.globalTree.SelectedNode.Text;

            //MessageBox.Show(path);

            if (path != "")
            {
                //MessageBox.Show(path);
                openFileDialogVobTree.InitialDirectory = System.IO.Path.GetDirectoryName(@path + @"/");
            }
            else
            {
                openFileDialogVobTree.InitialDirectory = @Directory.GetCurrentDirectory() + @"../_WORK/DATA/Worlds/";
            }

            openFileDialogVobTree.RestoreDirectory = true;

            //Extern_BlockMouse(true);

            if (openFileDialogVobTree.ShowDialog() == DialogResult.OK)
            {

                IntPtr ptrPathSave = Marshal.StringToHGlobalAnsi(Path.GetDirectoryName(openFileDialogVobTree.FileName));

                Extern_SetSettingStr(ptrPathSave, Marshal.StringToHGlobalAnsi("treeVobPath"));



                string filePath = openFileDialogVobTree.FileName;
                IntPtr filePathPtr = Marshal.StringToHGlobalAnsi(filePath);
                Extern_OpenVobTree(filePathPtr, false);

                Marshal.FreeHGlobal(ptrPathSave);
                Marshal.FreeHGlobal(filePathPtr);
            }
        }

        private void вставитьVobTreeГлобальноToolStripMenuItem_Click(object sender, EventArgs e)
        {


            openFileDialogVobTree.Filter = "Zen file (*.zen)|";
            IntPtr ptrPath = Extern_GetSettingStr(Marshal.StringToHGlobalAnsi("treeVobPath"));
            string path = Marshal.PtrToStringAnsi(ptrPath);

            string fileName = UnionNET.objTreeWin.globalTree.SelectedNode.Text;

            //MessageBox.Show(path);

            if (path != "")
            {
                //MessageBox.Show(path);
                openFileDialogVobTree.InitialDirectory = System.IO.Path.GetDirectoryName(@path + @"/");
            }
            else
            {
                openFileDialogVobTree.InitialDirectory = @Directory.GetCurrentDirectory() + @"../_WORK/DATA/Worlds/";
            }

            openFileDialogVobTree.RestoreDirectory = true;

            //Extern_BlockMouse(true);

            if (openFileDialogVobTree.ShowDialog() == DialogResult.OK)
            {

                IntPtr ptrPathSave = Marshal.StringToHGlobalAnsi(Path.GetDirectoryName(openFileDialogVobTree.FileName));

                Extern_SetSettingStr(ptrPathSave, Marshal.StringToHGlobalAnsi("treeVobPath"));



                string filePath = openFileDialogVobTree.FileName;
                IntPtr filePathPtr = Marshal.StringToHGlobalAnsi(filePath);
                Extern_OpenVobTree(filePathPtr, true);

                Marshal.FreeHGlobal(ptrPathSave);
                Marshal.FreeHGlobal(filePathPtr);
            }
        }

        private void удалитьВобToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalTree.SelectedNode == null)
            {
                return;
            }

            TreeNode node = globalTree.SelectedNode;

            string tag = node.Tag.ToString();

            if (tag.Length == 0 || tag == TAG_FOLDER)
            {
                return;
            }

            UnionNET.vobList.ClearListBox();
            uint vob = 0;

            uint.TryParse(tag, out vob);

            Extern_RemoveVob(vob);

        }

        private void globalTree_NodeMouseClick_1(object sender, TreeNodeMouseClickEventArgs e)
        {
            globalTree.SelectedNode = e.Node;
        }

        private void buttonTreeSort_Click(object sender, EventArgs e)
        {
            globalTree.Sort();
        }

        private void globalTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            previousSelectedNode = globalTree.SelectedNode;
        }
    }
}
