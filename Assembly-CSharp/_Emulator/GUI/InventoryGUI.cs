﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace _Emulator
{
    class InventoryGUIWindow
    {
        private Vector2 size = new Vector2(881f, 600f + 121f);
         
        private Rect crdCloseButton = new Rect(837f, 10f, 34f, 34f);

        private Rect crdBtnWeapon = new Rect(209f, 45f, 148f, 45f);

        private Rect crdBtnShooterTool = new Rect(365f, 45f, 148f, 45f);

        private Rect crdBtnOthers = new Rect(521f, 45f, 148f, 45f);

        private Vector2 crdItem = new Vector2(135f, 121f);

        private Rect crdItemActionOutline = new Rect(15f, 97f, 851f, 574f);

        private Rect crdItemWeaponOutline = new Rect(15f, 127f, 851f, 544f);

        private Rect crdWeaponKind = new Rect(85, 95f, 715f, 33f);

        private Rect crdMainKind = new Rect(104f, 133f, 164f, 28f);

        private Rect crdShooterToolList = new Rect(27f, 112f, 835f, 549f);

        private Rect crdWeaponList = new Rect(27f, 139f, 835f, 514f);

        private Rect crdMainWeaponList = new Rect(27f, 164f, 835f, 489f);

        private Rect crdItemBtn = new Rect(3f, 3f, 122f, 105f);

        private Vector2 crdItemStatus = new Vector2(120f, 98f);

        private Rect crdInit = new Rect(21f, 556f, 136f, 34f);

        private Rect crdLoadInventory = new Rect(30f, 677f, 136f, 34f);

        private Rect crdUpdateInventory = new Rect(163f, 677f, 136f, 34f);

        private Rect crdSaveInventory = new Rect(296f, 677f, 136f, 34f);

        private Rect crdRemoveWeapon = new Rect(435f, 677f, 136f, 34f);

        private Rect crdAddWeapon = new Rect(566f, 677f , 136f, 34f);

        private int curItem = -1;

        private TItem[] allItems;

        private Vector2 scrollPositionWeaponSlot = new Vector2(0f, 0f);

        private Vector2 scrollPositionWeapon = new Vector2(0f, 0f);

        private Vector2 scrollPositionShooterTool = new Vector2(0f, 0f);

        private float yOffset = 10f;

        private int mainTab = 0;

        private string[] weaponKindKey = new string[6]
        {
            "ALL",
            "MAIN_WEAPON",
            "AUX_WEAPON",
            "MELEE_WEAPON",
            "SPEC_WEAPON",
            "MY_EQUIP"
        };

        private string[] mainKindKey = new string[4]
        {
            "SUB_MACHINE_WPN",
            "ASSAULT_WPN",
            "HEAVY_WPN",
            "SNIPER_WPN"
        };

        private string[] otherKindKey = new string[8]
        {
            "CLOTH",
            "ACCESSORY",
            "CHARACTER",
            "TOOLBOX",
            "UPGRADE",
            "BUNDLE",
            "ETC",
            "MY_EQUIP"
        };

        private static string[] clothKinds = new string[4]
        {
            "ALL",
            "HELMET",
            "UPPER_CLOTH",
            "LOWER_CLOTH"
        };

        private static string[] accessoryKinds = new string[9]
        {
            "ALL",
            "HOLSTER",
            "SIDEAMMO",
            "WAISTBAG",
            "BAG",
            "MASK",
            "LEGCASE",
            "KIT",
            "BOTTLE"
        };

        private int weaponKind;

        private int mainKind;

        private int otherCatType;

        private int otherCatKind;

        Dictionary<string, TItem> activeDic;

        public TItem[] GetItemsByCat(int catType, int catKind, int category = -1)
        {
            List<TItem> list = new List<TItem>();

            if (catType != 1 && catType != 5)
            {
                if (catType == -1) // inventory
                {
                    foreach (Item it in ClientExtension.instance.inventory.equipment)
                    {
                        if (it != null && !it.IsWeaponSlotAble)
                        {
                            TItem tItem = TItemManager.Instance.Get<TItem>(it.Code);
                            if (tItem != null)
                                list.Add(tItem);
                        }
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, TItem> item in activeDic)
                    {
                        if ((item.Value.catType == catType || (catType == 0 && (item.Value.catType == 0 || item.Value.catType == 9))) && (catKind <= 0 || item.Value.catKind == catKind))
                        {
                            if (!item.Value.Name.Contains("fully upgraded"))
                                list.Add(item.Value);
                        }
                    }
                }

            }
            else
            {
                if (catKind == 5) // Inventory tab
                {
                    foreach (Item it in ClientExtension.instance.inventory.equipment)
                    {
                        if (it != null && it.IsEquipable && it.IsWeaponSlotAble)
                        {
                            TItem tItem = TItemManager.Instance.Get<TItem>(it.Code);
                            if (tItem != null)
                                list.Add(tItem);
                        }
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, TItem> item in activeDic)
                    {
                        if (item.Value != null)
                        {
                            if (item.Value.catType == catType && (catKind <= 0 || item.Value.catKind == catKind))
                            {
                                if (category >= 0)
                                {
                                    TWeapon tWeapon = (TWeapon)item.Value;
                                    if (tWeapon.cat == category)
                                    {
                                        if (!tWeapon.Name.Contains("fully upgraded"))
                                            list.Add(item.Value);
                                    }
                                }
                                else
                                {
                                    if (!item.Value.Name.Contains("fully upgraded"))
                                        list.Add(item.Value);
                                }
                            }
                        }
                    }
                }
            }

            list.Sort((TItem prev, TItem next) => prev.code.CompareTo(next.code));
            return list.ToArray();
        }

        private void ShowItemStatus(TItem item)
        {
            if (ClientExtension.instance.inventory.equipment.Exists(x => x.Template.code == item.code))
            {
                LabelUtil.TextOut(crdItemStatus, "IN INVENTORY", "MiniLabel", GlobalVars.Instance.GetByteColor2FloatColor(byte.MaxValue, 72, 48), Color.black, TextAnchor.MiddleRight);
            }
        }

        private void DrawItemIcon(TItem item, Rect crdIcon)
        {
            Color color = GUI.color;
            TextureUtil.DrawTexture(crdIcon, item.CurIcon(), ScaleMode.ScaleToFit);
            GUI.color = color;
        }

        private void DoTitle()
        {
            Vector2 pos = new Vector2(size.x / 2f, 10f);
            LabelUtil.TextOut(pos, "Inventory editor", "BigLabel", GlobalVars.Instance.txtMainColor, new Color(0f, 0f, 0f, 0f), TextAnchor.UpperCenter);
        }

        private void DoShooterTools()
        {
            GUI.Box(crdItemActionOutline, string.Empty, "LineBoxBlue");
            int num = 0;
            allItems = GetItemsByCat(5, -1);
            int num2 = allItems.Length;
            int num3 = 6;
            int num4 = num2 / num3;
            if (num2 % num3 > 0)
            {
                num4++;
            }
            float num5 = crdItem.x * (float)num3;
            if (num3 > 1)
            {
                num5 += (float)((num3 - 1) * 2);
            }
            float num6 = crdItem.y * (float)num4;
            if (num4 > 0)
            {
                num6 -= yOffset;
            }
            scrollPositionShooterTool = GUI.BeginScrollView(viewRect: new Rect(0f, 0f, crdShooterToolList.width - 20f, num6), position: crdShooterToolList, scrollPosition: scrollPositionShooterTool, alwaysShowHorizontal: false, alwaysShowVertical: false);
            float y = scrollPositionShooterTool.y;
            float num7 = y + crdShooterToolList.height;
            Rect position = new Rect(0f, 0f, crdItem.x, crdItem.y);
            num = 0;
            int num8 = 0;
            while (num < num2 && num8 < num4)
            {
                position.y = (float)num8 * crdItem.y;
                float y2 = position.y;
                float num9 = y2 + position.height;
                int num10 = 0;
                while (num < num2 && num10 < num3)
                {
                    if (num9 >= y && y2 <= num7)
                    {
                        position.x = (float)num10 * (crdItem.x + 2f);
                        GUI.BeginGroup(position);
                        TItem tItem = allItems[num];
                        string str = "BtnItem";
                        if (tItem.season == 2)
                        {
                            str = "BtnItem2";
                        }
                        if (GUI.Button(crdItemBtn, new GUIContent(string.Empty, allItems[num].Name), str))
                        {
                            curItem = num;
                        }
                        DrawItemIcon(crdIcon: new Rect(crdItemBtn.x + 4f, crdItemBtn.y + 14f, (float)(int)((double)tItem.CurIcon().width * 0.65), (float)(int)((double)tItem.CurIcon().height * 0.65)), item: allItems[num]);
                        Color color = GUI.color;
                        GUI.color = GlobalVars.Instance.txtMainColor;
                        GUI.Label(crdItemBtn, tItem.Name, "MiniLabel");
                        GUI.color = color;
                        ShowItemStatus(allItems[num]);
                        if (num == curItem)
                        {
                            GUI.Box(new Rect(crdItemBtn.x - 3f, crdItemBtn.y - 3f, crdItemBtn.width + 6f, crdItemBtn.height + 6f), string.Empty, "BtnItemF");
                        }
                        GUI.EndGroup();
                    }
                    num++;
                    num10++;
                }
                num8++;
            }
            GUI.EndScrollView();
            DoWeaponButtons((0 > curItem || curItem >= allItems.Length) ? null : allItems[curItem]);
        }

        private void DoWeaponTab()
        {
            string[] array = new string[weaponKindKey.Length];
            for (int i = 0; i < weaponKindKey.Length; i++)
            {
                array[i] = StringMgr.Instance.Get(weaponKindKey[i]);
            }
            weaponKind = GUI.SelectionGrid(crdWeaponKind, weaponKind, array, array.Length, "PopTab");
            if (weaponKind == 1)
            {
                for (int j = 0; j < mainKindKey.Length; j++)
                {
                    Rect rect = new Rect(crdMainKind);
                    rect.x += (float)j * (crdMainKind.width + 6f);
                    if (GlobalVars.Instance.MyButton(rect, StringMgr.Instance.Get(mainKindKey[j]), "BtnBlue"))
                    {
                        mainKind = j;
                    }
                    if (mainKind == j)
                    {
                        GUI.Box(rect, string.Empty, "BtnBlueF");
                    }
                }
            }
        }

        private void DoWeapons()
        {
            GUI.Box(crdItemWeaponOutline, string.Empty, "LineBoxBlue");

            int num = 0;
            int[] catKind = new int[4] { 3, 1, 0, 2 };

            allItems = GetItemsByCat(1, weaponKind, (weaponKind != 1) ? (-1) : catKind[mainKind]);
            int allItemCount = allItems.Length;
            int itemsPerRow = 6;
            int curRow = allItemCount / itemsPerRow;
            if (allItemCount % itemsPerRow > 0)
            {
                curRow++;
            }
            float xPosition = crdItem.x * (float)itemsPerRow;
            if (itemsPerRow > 1)
            {
                xPosition += (float)((itemsPerRow - 1) * 2);
            }
            float yPosition = crdItem.y * (float)curRow;
            if (curRow > 0)
            {
                yPosition -= this.yOffset;
            }
            Rect position = crdWeaponList;
            if (weaponKind == 1)
            {
                position = crdMainWeaponList;
            }

            scrollPositionWeapon = GUI.BeginScrollView(viewRect: new Rect(0f, 0f, position.width - 20f, yPosition), position: position, scrollPosition: scrollPositionWeapon, alwaysShowHorizontal: false, alwaysShowVertical: false);
            float y = scrollPositionWeapon.y;
            float num7 = y + position.height;
            Rect position2 = new Rect(0f, 0f, crdItem.x, crdItem.y);
            num = 0;
            int num8 = 0;
            while (num < allItemCount && num8 < curRow)
            {
                position2.y = (float)num8 * crdItem.y;
                float y2 = position2.y;
                float num9 = y2 + position2.height;
                int num10 = 0;
                while (num < allItemCount && num10 < itemsPerRow)
                {
                    if (num9 >= y && y2 <= num7)
                    {
                        position2.x = (float)num10 * (crdItem.x + 2f);
                        GUI.BeginGroup(position2);
                        TItem tItem = allItems[num];
                        string str = "BtnItem";
                        if (tItem.season == 2)
                        {
                            str = "BtnItem2";
                        }
                        if (GUI.Button(crdItemBtn, new GUIContent(string.Empty, allItems[num].Name), str))
                        {
                            curItem = num;
                        }
                        DrawItemIcon(crdIcon: new Rect(crdItemBtn.x + 4f, crdItemBtn.y + 14f, (float)(int)((double)tItem.CurIcon().width * 0.65), (float)(int)((double)tItem.CurIcon().height * 0.65)), item: allItems[num]);
                        Color color = GUI.color;
                        GUI.color = GlobalVars.Instance.txtMainColor;
                        GUI.Label(crdItemBtn, tItem.Name, "MiniLabel");
                        GUI.color = color;
                        ShowItemStatus(allItems[num]);
                        if (num == curItem)
                        {
                            GUI.Box(new Rect(crdItemBtn.x - 3f, crdItemBtn.y - 3f, crdItemBtn.width + 6f, crdItemBtn.height + 6f), string.Empty, "BtnItemF");
                        }
                        GUI.EndGroup();
                    }
                    num++;
                    num10++;
                }
                num8++;
            }
            
            GUI.EndScrollView();
            DoWeaponTab();

            DoWeaponButtons((0 > curItem || curItem >= allItems.Length) ? null : allItems[curItem]);
        }

        private void DoWeaponButtons(TItem item)
        {
            GUIContent content = new GUIContent("ADD", GlobalVars.Instance.iconEquip);
            if (GlobalVars.Instance.MyButton3(crdAddWeapon, content, "BtnAction") && item != null)
            {
                GlobalVars.Instance.PlaySoundItemInstall();
                ClientExtension.instance.inventory.AddItem(item, true);

                ClientExtension.instance.inventory.UpdateCSV();
                ClientExtension.instance.SendInventoryCSV();
            }
            content = new GUIContent("REMOVE", GlobalVars.Instance.iconCancel);
            if (GlobalVars.Instance.MyButton3(crdRemoveWeapon, content, "BtnAction"))
            {
                // find a way to convert TItem to Item
                ClientExtension.instance.inventory.equipment.ForEach(delegate (Item it)
                {
                    if (it.Code == item.code)
                        ClientExtension.instance.inventory.RemoveItem(it);
                });

                ClientExtension.instance.inventory.UpdateCSV();
                ClientExtension.instance.SendInventoryCSV();
            }
            content = new GUIContent("SAVE");
            if (GlobalVars.Instance.MyButton3(crdSaveInventory, content, "BtnAction"))
            {
                ClientExtension.instance.inventory.UpdateCSV();
                ClientExtension.instance.inventory.Save();
                ClientExtension.instance.SendInventoryCSV();
            }
            content = new GUIContent("UPDATE");
            if (GlobalVars.Instance.MyButton3(crdUpdateInventory, content, "BtnAction"))
            {
                ClientExtension.instance.inventory.UpdateCSV();
                ClientExtension.instance.SendInventoryCSV();
            }
            content = new GUIContent("LOAD");
            if (GlobalVars.Instance.MyButton3(crdLoadInventory, content, "BtnAction"))
            {
                ClientExtension.instance.inventory.LoadInventoryFromDisk();
                ClientExtension.instance.inventory.Sort();
                ClientExtension.instance.SendInventoryCSV();
            }
        }

        private void DoOthersTab()
        {
            string[] array = new string[otherKindKey.Length];
            for (int i = 0; i < otherKindKey.Length; i++)
            {
                array[i] = StringMgr.Instance.Get(otherKindKey[i]);
            }
            otherCatType = GUI.SelectionGrid(crdWeaponKind, otherCatType, array, array.Length, "PopTab");
            if (otherCatType == 0) // Clothing
            {
                if (otherCatKind > 3)
                    otherCatKind = 3;
                for (int j = 0; j < clothKinds.Length; j++)
                {
                    Rect rect = new Rect(crdMainKind);
                    rect.x += (float)j * (crdMainKind.width + 6f);
                    if (GlobalVars.Instance.MyButton(rect, StringMgr.Instance.Get(clothKinds[j]), "BtnBlue"))
                    {
                        otherCatKind = j;
                    }
                    if (otherCatKind == j)
                    {
                        GUI.Box(rect, string.Empty, "BtnBlueF");
                    }
                }
            }
            if (otherCatType == 1) // Accessories
            {
                float maxWidth = (660 - 6 * accessoryKinds.Length) / accessoryKinds.Length;
                for (int j = 0; j < accessoryKinds.Length; j++)
                {
                    Rect rect = new Rect(crdMainKind);
                    rect.width = maxWidth;
                    rect.x += (float)j * (maxWidth + 6f);
                    if (GlobalVars.Instance.MyButton(rect, StringMgr.Instance.Get(accessoryKinds[j]), "BtnBlue"))
                    {
                        otherCatKind = j;
                    }
                    if (otherCatKind == j)
                    {
                        GUI.Box(rect, string.Empty, "BtnBlueF");
                    }
                }
            }
        }

        private void DoOthers()
        {
            GUI.Box(crdItemWeaponOutline, string.Empty, "LineBoxBlue");

            int num = 0;
            int[] catTypes = new int[8] { 2, 3, 4, 6, 7, 8, 0, -1 };
            int[] catKinds = new int[10] { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1 };
            if (otherCatType == 0)
            {
                catKinds = new int[4] { 0, 1, 2, 3 };
            }
            else if (otherCatType == 1)
            {
                catKinds = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            }

            allItems = GetItemsByCat(catTypes[otherCatType], (otherCatKind < 2 ? catKinds[otherCatKind] : -1));
            
            int allItemCount = allItems.Length;
            int itemsPerRow = 6;
            int curRow = allItemCount / itemsPerRow;
            if (allItemCount % itemsPerRow > 0)
            {
                curRow++;
            }
            float xPosition = crdItem.x * (float)itemsPerRow;
            if (itemsPerRow > 1)
            {
                xPosition += (float)((itemsPerRow - 1) * 2);
            }
            float yPosition = crdItem.y * (float)curRow;
            if (curRow > 0)
            {
                yPosition -= this.yOffset;
            }
            Rect position = crdWeaponList;
            if (otherCatType == 0 || otherCatType == 1)
            {
                position = crdMainWeaponList;
            }

            scrollPositionWeapon = GUI.BeginScrollView(viewRect: new Rect(0f, 0f, position.width - 20f, yPosition), position: position, scrollPosition: scrollPositionWeapon, alwaysShowHorizontal: false, alwaysShowVertical: false);
            float y = scrollPositionWeapon.y;
            float num7 = y + position.height;
            Rect position2 = new Rect(0f, 0f, crdItem.x, crdItem.y);
            num = 0;
            int num8 = 0;
            while (num < allItemCount && num8 < curRow)
            {
                position2.y = (float)num8 * crdItem.y;
                float y2 = position2.y;
                float num9 = y2 + position2.height;
                int num10 = 0;
                while (num < allItemCount && num10 < itemsPerRow)
                {
                    if (num9 >= y && y2 <= num7)
                    {
                        position2.x = (float)num10 * (crdItem.x + 2f);
                        GUI.BeginGroup(position2);
                        TItem tItem = allItems[num];
                        string str = "BtnItem";
                        if (tItem.season == 2)
                        {
                            str = "BtnItem2";
                        }
                        if (GUI.Button(crdItemBtn, new GUIContent(string.Empty, allItems[num].Name), str))
                        {
                            curItem = num;
                        }
                        DrawItemIcon(crdIcon: new Rect(crdItemBtn.x + 4f, crdItemBtn.y + 14f, (float)(int)((double)tItem.CurIcon().width * 0.65), (float)(int)((double)tItem.CurIcon().height * 0.65)), item: allItems[num]);
                        Color color = GUI.color;
                        GUI.color = GlobalVars.Instance.txtMainColor;
                        GUI.Label(crdItemBtn, tItem.Name, "MiniLabel");
                        GUI.color = color;
                        ShowItemStatus(allItems[num]);
                        if (num == curItem)
                        {
                            GUI.Box(new Rect(crdItemBtn.x - 3f, crdItemBtn.y - 3f, crdItemBtn.width + 6f, crdItemBtn.height + 6f), string.Empty, "BtnItemF");
                        }
                        GUI.EndGroup();
                    }
                    num++;
                    num10++;
                }
                num8++;
            }

            GUI.EndScrollView();

            DoOthersTab();
            DoWeaponButtons((0 > curItem || curItem >= allItems.Length) ? null : allItems[curItem]);
        }

        private void DoMainTab()
        {
            if (GlobalVars.Instance.MyButton(crdBtnWeapon, StringMgr.Instance.Get("WEAPON"), "BtnBlue"))
            {
                mainTab = 0;
            }
            if (GlobalVars.Instance.MyButton(crdBtnShooterTool, StringMgr.Instance.Get("ACTIONPANEL"), "BtnBlue"))
            {
                mainTab = 1;
            }
            if (GlobalVars.Instance.MyButton(crdBtnOthers, "Other", "BtnBlue"))
            {
                mainTab = 2;
            }
            switch (mainTab)
            {
                case 0:
                    GUI.Box(crdBtnWeapon, string.Empty, "BtnBlueF");
                    break;
                case 1:
                    GUI.Box(crdBtnShooterTool, string.Empty, "BtnBlueF");
                    break;
                case 2:
                    GUI.Box(crdBtnOthers, string.Empty, "BtnBlueF");
                    break;
            }
        }

        public bool Start()
        {
            if (activeDic == null)
            {
                activeDic = TItemManager.Instance.dic.OrderByDescending(x => x.Value.slot).ToDictionary(x => x.Key, x => x.Value);
            }
            bool result = false;
            GUISkin skin = GUI.skin;
            GUI.skin = GUISkinFinder.Instance.GetGUISkin();
            Rect position = new Rect((GlobalVars.Instance.ScreenRect.width - size.x) / 2f, (GlobalVars.Instance.ScreenRect.height - size.y) / 2f, size.x, size.y);
            GUI.Box(position, string.Empty, "Window");
            GUI.BeginGroup(position);
            DoTitle();
            DoMainTab();
            switch (mainTab)
            {
                case 0:
                    DoWeapons();
                    break;
                case 1:
                    DoShooterTools();
                    break;
                case 2:
                    DoOthers();
                    break;
            }
            if (GlobalVars.Instance.MyButton(crdCloseButton, string.Empty, "BtnClose") || GlobalVars.Instance.IsEscapePressed())
            {
                result = true;
            }
            GUI.EndGroup();
            if (!ContextMenuManager.Instance.IsPopup)
            {
                WindowUtil.EatEvent();
            }
            GUI.skin = skin;
            return result;
        }
    }

    class InventoryGUI : MonoBehaviour
    {
        public static InventoryGUI instance;

        public bool hidden = true;

        private InventoryGUIWindow wnd;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
                hidden = !hidden;
        }

        private void Start()
        {
            if (wnd == null)
                wnd = new InventoryGUIWindow();
        }

        private void OnGUI()
        {
            if (!hidden)
            {
                if (ClientExtension.instance.clientConnected && TItemManager.Instance != null && ClientExtension.instance.inventory != null)
                {
                    hidden = wnd.Start();
                }
            }
        }
    }
}