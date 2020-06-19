﻿
using SpacerUnion.Common;
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

        public static TreeNode previousSelectedNode = null;


        

        public static Dictionary<uint, TreeEntry> globalEntries = new Dictionary<uint, TreeEntry>();
        public static Dictionary<uint, TreeEntry> tempEntries = new Dictionary<uint, TreeEntry>();
        static bool nextAfterEventBlocked = false;
        static TreeNode lastSelectedNode = null;
        static bool IsWaypointReload = false;

        public ObjTree()
        {
            InitializeComponent();
        }

        public void UpdateLang()
        {
            this.Text = Localizator.Get("WIN_TREE_TITLE");
            buttonCollapse.Text = Localizator.Get("buttonCollapse");
            buttonExpand.Text = Localizator.Get("buttonExpand");
            buttonTreeSort.Text = Localizator.Get("buttonTreeSort");

            contextMenuStripTree.Items[0].Text = Localizator.Get("CONTEXTMENU_TREE_INSERT_VOBTREE_PARENT");
            contextMenuStripTree.Items[1].Text = Localizator.Get("CONTEXTMENU_TREE_INSERT_VOBTREE_GLOBAL");
            contextMenuStripTree.Items[2].Text = Localizator.Get("CONTEXTMENU_TREE_SAVE_VOBTREE");
            contextMenuStripTree.Items[3].Text = Localizator.Get("CONTEXTMENU_TREE_REMOVE_VOB");
        }

        

  
        public static int CreateAndGetFolder(string className)
        {
            TreeNodeCollection nodes = SpacerNET.objTreeWin.globalTree.Nodes;

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
                newNode.Tag = Constants.TAG_FOLDER;
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

            if (node.Tag.ToString() == Constants.TAG_FOLDER)
            {
                node.ImageIndex = 0;
                node.SelectedImageIndex = 0;
            }

        }

        public static void AddVobToNodes(TreeEntry entry)
        {
            TreeNodeCollection nodes = SpacerNET.objTreeWin.globalTree.Nodes;

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
        public static void ClearAllEntries()
        {
            SpacerNET.objTreeWin.globalTree.Nodes.Clear();
            ObjTree.globalEntries.Clear();
        }


        // Обновление родителя для сущ. воба
        [DllExport]
        public static void updateParentAddNode(uint ptr, uint ptrParent)
        {
            if (ptr == 0)
            {
                return;
            }

            TreeEntry entryParent = null;
            TreeEntry entry = null;

            try
            {
                entryParent = globalEntries
                    .Where(x => x.Value.zCVob == ptrParent)
                    .Select(pair => pair.Value)
                    .FirstOrDefault();

                entry = globalEntries
                    .Where(x => x.Value.zCVob == ptr)
                    .Select(pair => pair.Value)
                    .FirstOrDefault();
            }
            catch
            {
                Utils.Error("updateParentAddNode fail. No parent found. Addr vob: " + Utils.ToHex(ptr));
            }

            if (entryParent != null && entry != null)
            {
                ConsoleEx.WriteLineGreen("Обновляю родителя для воба: " + entry.name);
                entry.parent = ptrParent;
                entry.parentEntry = entryParent;
                entryParent.childs.Add(entry);

                TreeNodeCollection nodes = SpacerNET.objTreeWin.globalTree.Nodes;

                AddVobToNodes(entry);
            }

            
            //ConsoleEx.WriteLineGreen("Всего вобов в списке: " + globalEntries.Count);
            countNodeView = 0;
            CalcNodesCount(SpacerNET.objTreeWin.globalTree.Nodes);
           // ConsoleEx.WriteLineGreen("C#: Всего узлов TreeView: " + countNodeView);


        }
        [DllExport]
        public static void UpdateParentRemoveNode(uint ptr)
        {
            if (ptr == 0)
            {
                return;
            }

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
                Utils.Error("C#: UpdateParentRemoveNode fail. No vob found in globalList. Addr: " + Utils.ToHex(ptr));
            }

            if (entry != null)
            {
                if (entry.node != null)
                {
                    entry.node.Remove();

                    if (entry.parentEntry != null)
                    {
                        entry.parentEntry.childs.Remove(entry);
                    }
                }
            }
        }



        [DllExport]
        public static void UpdateVobName(uint ptr)
        {
            if (ptr == 0)
            {
                return;
            }

            Stopwatch s = new Stopwatch();
            s.Start();

            string name = Imports.Stack_PeekString();

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
                ConsoleEx.WriteLineRed("UpdateName fail. No vob found in globalList. Addr: " + Utils.ToHex(ptr));

                Utils.WriteToFile(String.Format("UpdateName: No vob found in globalList addr: {0}, name {1}", Utils.ToHex(ptr), name));
            }


            if (entry != null)
            {
                if (entry.node != null)
                {
                    entry.name = name;
                    entry.node.Text = name;
                    SpacerNET.objTreeWin.globalTree.SelectedNode = entry.node;
                }
            }


            ConsoleEx.WriteLineGreen("UpdateName for vob: " + Utils.ToHex(ptr));


        }


        [DllExport]
        public static void SelectNode(uint ptr)
        {
            TreeEntry entry = null;


            entry = globalEntries
                    .Where(x => x.Value.zCVob == ptr)
                    .Select(pair => pair.Value)
                    .FirstOrDefault();

            if (entry != null)
            {
                if (entry.node != null)
                {
                    SpacerNET.objTreeWin.globalTree.SelectedNode = entry.node;
                    SpacerNET.objTreeWin.globalTree.SelectedNode.ExpandAll();
                }
            }
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


            ConsoleEx.WriteLineGreen("OnSelectVob: " + Utils.ToHex(ptr));


            if (entry != null)
            {
                if (entry.node != null)
                {
                    SpacerNET.objTreeWin.globalTree.SelectedNode = entry.node;
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
                ConsoleEx.WriteLineGreen("Remove node: " + entry.node.Text + " Parent: " + Utils.ToHex(entry.parent));
                entry.node.Remove();
            }
            else
            {
                ConsoleEx.WriteLineGreen("Remove node failure. Node is null");
            }

            entry.childs.Clear();
            entry.toDelete = true;
            entry.node = null;
        }

        static int countNodeView = 0;

        public static void CalcNodesCount(TreeNodeCollection nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                CalcNodesCount(nodes[i].Nodes);

                if (nodes[i].Tag != null && nodes[i].Tag.ToString() != Constants.TAG_FOLDER)
                {
                    countNodeView++;
                }

            }


        }

        [DllExport]
        public static void ReloadWaypoint()
        {
            //ConsoleEx.WriteLineGreen("C#: Перестраиваю список вейпоинтов в интерфейсе: Кол-во вобов в списке " + globalEntries.Count);

            foreach (var entry in tempEntries)
            {
                AddVobToNodes(entry.Value);
            }

            IsWaypointReload = false;
            tempEntries.Clear();

            //ConsoleEx.WriteLineGreen("C#: Дерево вейпоинтов обновлено заполнено. Всего записей: " + globalEntries.Count);
        }
        
        [DllExport]
        public static void ClearWaypoints()
        {

            //ConsoleEx.WriteLineGreen("C#: Начало очистки вейпоинтов: Кол-во вобов в списке " + globalEntries.Count);


            TreeNode node = SpacerNET.objTreeWin.globalTree.SelectedNode;

            // Если есть выделенный объект и это вейпоинт, то снимает выделение, потому что Node будет удален, иначе вылет
            if (node != null)
            {
                string tag = node.Tag.ToString();

                if (tag != Constants.TAG_FOLDER && tag.Length > 0)
                {
                    uint addr = Convert.ToUInt32(node.Tag);

                    if (globalEntries[addr].className == "zCVobWaypoint")
                    {
                        SpacerNET.objTreeWin.globalTree.SelectedNode = null;
                    }
                }
            }

         
            var waypointsNodesList = globalEntries
                        .Where(pair => pair.Value.className == "zCVobWaypoint")
                        .ToDictionary(pair => pair.Key, pair => pair.Value);



            foreach (var entry in waypointsNodesList)
            {
                if (entry.Value.node != null)
                {
                    entry.Value.node.Remove();
                }
            }

            globalEntries = globalEntries
                        .Where(pair => pair.Value.className != "zCVobWaypoint")
                        .ToDictionary(pair => pair.Key, pair => pair.Value);


            IsWaypointReload = true;
            tempEntries.Clear();
           // ConsoleEx.WriteLineGreen("Clean waypoints. All vobs count: " + globalEntries.Count);

        }

        [DllExport]
        public static void OnVobRemove(uint vob)
        {
            TreeNodeCollection nodes = SpacerNET.objTreeWin.globalTree.Nodes;

            //ConsoleEx.WriteLineGreen("OnVobRemove: " + Utils.ToHex(vob));
            //ConsoleEx.WriteLineGreen("All vobs count: " + globalEntries.Count);

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

                if (globalEntries.ContainsKey(vob))
                {
                    Utils.Error("WTF? I have removed the vob with such key: " + Utils.ToHex(vob) + " " + entry.name);
                }

                if (entries.Count > 1)
                {
                    Utils.Error("Warning! I found more than 1 entries with same Vob addr! Count: " + entries.Count);
                }
            }

            SpacerNET.vobList.ClearListBox();
            ObjectsWindow.CleanProps();

            ConsoleEx.WriteLineGreen("OnVobRemove: All vobs count: " + globalEntries.Count);
            countNodeView = 0;
            //CalcNodesCount(SpacerNET.objTreeWin.globalTree.Nodes);
            //ConsoleEx.WriteLineGreen("All TreeView nodes count: " + countNodeView);
            
            //Console.WriteLine("=============================");


        }

       
        [DllExport]
        public static void OnVobInsert(uint vob, uint parent, int isNodeBlocked, bool select)
        {
            string name = Imports.Stack_PeekString();
            string className = Imports.Stack_PeekString();

            TreeNodeCollection nodes = SpacerNET.objTreeWin.globalTree.Nodes;

            Console.WriteLine("");
            ConsoleEx.WriteLineGreen("OnVobInsert: " + name);
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


                msg += "\nglobalEntries Count: " + globalEntries.Count.ToString();


                countNodeView = 0;
                CalcNodesCount(SpacerNET.objTreeWin.globalTree.Nodes);


                msg += "\nNodes Count: " + countNodeView.ToString();

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

                Utils.Error(msg);

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

                
                if (select)
                {
                    SpacerNET.objTreeWin.globalTree.SelectedNode = node;
                }
                else
                {
                    //ConsoleEx.WriteLineRed("No select");
                }
               
                
                
                Console.WriteLine("OnVobInsert globally: " + name + " parent: " + Utils.ToHex(parent) + " className: " + className);
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


                    if (select)
                    {
                        SpacerNET.objTreeWin.globalTree.SelectedNode = node;
                    }
                    else
                    {
                        //ConsoleEx.WriteLineRed("No select");
                    }



                }
                else
                {

                    string msg = "OnVobInsert: parent node is null. Vob  "
                    + entry.parentEntry.name;


                    Utils.Error(msg);
                }

            }
            else
            {
                Utils.Error("OnVobInsert: parent entry is null");
            }


            ConsoleEx.WriteLineGreen("Vobs count: " + globalEntries.Count);
            countNodeView = 0;
            CalcNodesCount(SpacerNET.objTreeWin.globalTree.Nodes);
            ConsoleEx.WriteLineGreen("TreeView Nodes count: " + countNodeView);
        }


        [DllExport]
        public static void CreateTree()
        {
            noParentCount = 0;

            SpacerNET.objTreeWin.globalTree.Visible = false;

            //globalEntries = globalEntries.OrderBy(x => x.Value.name).ToDictionary(x => x.Key, x => x.Value);

            foreach (var entry in globalEntries)
            {
                AddVobToNodes(entry.Value);
            }

            ConsoleEx.WriteLineGreen("Tree is ready. GlobalEntries count: " + globalEntries.Count);
            SpacerNET.objTreeWin.globalTree.Visible = true;
            Application.DoEvents();

        }

        [DllExport]
        public static void AddGlobalEntry(uint vob, uint parent)
        {

            string name = Imports.Stack_PeekString();
            string className = Imports.Stack_PeekString();


            TreeNodeCollection nodes = SpacerNET.objTreeWin.globalTree.Nodes;

            TreeEntry entry = new TreeEntry();

            entry.name = name;
            entry.parent = parent;

            entry.zCVob = vob;
            entry.className = className;
            entry.isLevel = entry.className == "zCVobLevelCompo";


            try
            {
                globalEntries.Add(vob, entry);

                if (IsWaypointReload)
                {
                    tempEntries.Add(vob, entry);
                }
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

           


            if (tag.Length == 0 || tag == Constants.TAG_FOLDER)
            {
                return;
            }

            uint addr = Convert.ToUInt32(node.Tag);

            ConsoleEx.WriteLineGreen("OnSelectDoubleClick node: vob " + Utils.ToHex(addr));

            Imports.Extern_SelectVob(addr);
            SpacerNET.form.Focus();
        }

        private void globalTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView tree = sender as TreeView;

            if (nextAfterEventBlocked)
            {
                ConsoleEx.WriteLineGreen("AfterSelect event was aborted.");
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
                if (previousSelectedNode.Tag.ToString() == Constants.TAG_FOLDER)
                {
                    previousSelectedNode.SelectedImageIndex = 0;
                }
                else
                {
                    previousSelectedNode.SelectedImageIndex = 1;
                }
                
            }

            string tag = node.Tag.ToString();

            if (tag.Length == 0 || tag == Constants.TAG_FOLDER)
            {
                return;
            }

            node.SelectedImageIndex = 4;




            uint addr = Convert.ToUInt32(node.Tag);

            ConsoleEx.WriteLineGreen("AfterSelect node: vob " + Utils.ToHex(addr));

            if (node.Text.Contains("zCVobWaypoint") || node.Text.Contains("zCVobSpot"))
            {
                ObjectsWin.ChangeTab(4);
            }

            Imports.Extern_SelectVobSync(addr);

            SpacerNET.form.Focus();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (SpacerNET.objTreeWin.globalTree.SelectedNode == null)
            {
                return;
            }

            TreeNode node = globalTree.SelectedNode;

            string tag = node.Tag.ToString();

            if (tag.Length == 0 || tag == Constants.TAG_FOLDER)
            {
                return;
            }


            saveFileDialogVobTree.Filter = Constants.FILE_FILTER_OPEN_ZEN;



            Imports.Stack_PushString("treeVobPath");
            Imports.Extern_GetSettingStr();
            string path = Imports.Stack_PeekString();

            string fileName = SpacerNET.objTreeWin.globalTree.SelectedNode.Text;

            //MessageBox.Show(path);


            saveFileDialogVobTree.InitialDirectory = Utils.GetInitialDirectory(path);


            saveFileDialogVobTree.RestoreDirectory = true;
            saveFileDialogVobTree.FileName = fileName + ".ZEN";

            //Imports.Extern_BlockMouse(true);

            if (saveFileDialogVobTree.ShowDialog() == DialogResult.OK)
            {



                Imports.Stack_PushString(Utils.FixPath(Path.GetDirectoryName(Utils.FixPath(saveFileDialogVobTree.FileName))));
                Imports.Stack_PushString("treeVobPath");
                Imports.Extern_SetSettingStr();


                string filePath = saveFileDialogVobTree.FileName;
                Imports.Stack_PushString(filePath);
                Imports.Extern_SaveVobTree();
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

            if (tag.Length == 0 || tag == Constants.TAG_FOLDER)
            {
                return;
            }


            openFileDialogVobTree.Filter = Constants.FILE_FILTER_OPEN_ZEN;
    


            Imports.Stack_PushString("treeVobPath");
            Imports.Extern_GetSettingStr();
            string path = Imports.Stack_PeekString();

            string fileName = SpacerNET.objTreeWin.globalTree.SelectedNode.Text;

            //MessageBox.Show(path);

            openFileDialogVobTree.InitialDirectory = Utils.GetInitialDirectory(path);

            openFileDialogVobTree.RestoreDirectory = true;

            //Imports.Extern_BlockMouse(true);

            if (openFileDialogVobTree.ShowDialog() == DialogResult.OK)
            {


                Imports.Stack_PushString(Utils.FixPath(Path.GetDirectoryName(Utils.FixPath(openFileDialogVobTree.FileName))));
                Imports.Stack_PushString("treeVobPath");
                Imports.Extern_SetSettingStr();


                string filePath = openFileDialogVobTree.FileName;
                Imports.Stack_PushString(filePath);
                Imports.Extern_OpenVobTree(false);

            }

            
        }

        private void вставитьVobTreeГлобальноToolStripMenuItem_Click(object sender, EventArgs e)
        {


            openFileDialogVobTree.Filter = Constants.FILE_FILTER_OPEN_ZEN;



            Imports.Stack_PushString("treeVobPath");
            Imports.Extern_GetSettingStr();
            string path = Imports.Stack_PeekString();

            string fileName = SpacerNET.objTreeWin.globalTree.SelectedNode.Text;

            //MessageBox.Show(path);

            openFileDialogVobTree.InitialDirectory = Utils.GetInitialDirectory(path);

            openFileDialogVobTree.RestoreDirectory = true;

            //Imports.Extern_BlockMouse(true);

            if (openFileDialogVobTree.ShowDialog() == DialogResult.OK)
            {


                Imports.Stack_PushString(Utils.FixPath(Path.GetDirectoryName(Utils.FixPath(openFileDialogVobTree.FileName))));
                Imports.Stack_PushString("treeVobPath");
                Imports.Extern_SetSettingStr();


                string filePath = openFileDialogVobTree.FileName;
                Imports.Stack_PushString(filePath);
                Imports.Extern_OpenVobTree(true);

                if (SpacerNET.objTreeWin.globalTree.SelectedNode != null)
                {
                    SpacerNET.objTreeWin.globalTree.SelectedNode.ExpandAll();
                }

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

            if (tag.Length == 0 || tag == Constants.TAG_FOLDER)
            {
                return;
            }

            SpacerNET.vobList.ClearListBox();
            uint vob = 0;

            uint.TryParse(tag, out vob);

            Imports.Extern_RemoveVob(vob);

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

        private void ObjTree_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.TreeWinLocation != null)
            {
                this.Location = Properties.Settings.Default.TreeWinLocation;
            }

        }

        private void ObjTree_Shown(object sender, EventArgs e)
        {
        }
    }
}
