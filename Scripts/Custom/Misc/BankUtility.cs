﻿using System;
using Server.Items;

namespace Server.Misc
{
    public class BankUtility
    {
        public static void OnSpeech(SpeechEventArgs e, Point3D location)
        {
            if (!e.Handled && e.Mobile.InRange(location, 12))
            {
                for (int i = 0; i < e.Keywords.Length; ++i)
                {
                    int keyword = e.Keywords[i];

                    switch (keyword)
                    {
                        case 0x0000: // *withdraw*
                            {
                                e.Handled = true;

                                if (e.Mobile.Criminal)
                                {
                                    e.Mobile.Say(500389); // I will not do business with a criminal!
                                    break;
                                }

                                string[] split = e.Speech.Split(' ');

                                if (split.Length >= 2)
                                {
                                    int amount;

                                    try
                                    {
                                        amount = Convert.ToInt32(split[1]);
                                    }
                                    catch
                                    {
                                        break;
                                    }

                                    if (amount > 5000)
                                    {
                                        e.Mobile.Say(500381); // Thou canst not withdraw so much at one time!
                                    }
                                    else if (amount > 0)
                                    {
                                        BankBox box = e.Mobile.FindBankNoCreate();

                                        if (box == null || !box.ConsumeTotal(typeof(Gold), amount))
                                        {
                                            e.Mobile.Say(500384); // Ah, art thou trying to fool me? Thou hast not so much gold!
                                        }
                                        else
                                        {
                                            e.Mobile.AddToBackpack(new Gold(amount));

                                            e.Mobile.Say(1010005); // Thou hast withdrawn gold from thy account.
                                        }
                                    }
                                }

                                break;
                            }
                        case 0x0001: // *balance*
                            {
                                e.Handled = true;

                                if (e.Mobile.Criminal)
                                {
                                    e.Mobile.Say(500389); // I will not do business with a criminal!
                                    break;
                                }

                                BankBox box = e.Mobile.FindBankNoCreate();

                                if (box != null)
                                    e.Mobile.Say(1042759, box.TotalGold.ToString()); // Thy current bank balance is ~1_AMOUNT~ gold.
                                else
                                    e.Mobile.Say(1042759, "0"); // Thy current bank balance is ~1_AMOUNT~ gold.

                                break;
                            }
                        case 0x0002: // *bank*
                            {
                                e.Handled = true;

                                if (e.Mobile.Criminal)
                                {
                                    e.Mobile.Say(500378); // Thou art a criminal and cannot access thy bank box.
                                    break;
                                }

                                e.Mobile.BankBox.Open();

                                break;
                            }
                        case 0x0003: // *check*
                            {
                                e.Handled = true;

                                if (e.Mobile.Criminal)
                                {
                                    e.Mobile.Say(500389); // I will not do business with a criminal!
                                    break;
                                }

                                string[] split = e.Speech.Split(' ');

                                if (split.Length >= 2)
                                {
                                    int amount;

                                    try
                                    {
                                        amount = Convert.ToInt32(split[1]);
                                    }
                                    catch
                                    {
                                        break;
                                    }

                                    if (amount < 5000)
                                    {
                                        e.Mobile.Say(1010006); // We cannot create checks for such a paltry amount of gold!
                                    }
                                    else if (amount > 1000000)
                                    {
                                        e.Mobile.Say(1010007); // Our policies prevent us from creating checks worth that much!
                                    }
                                    else
                                    {
                                        BankCheck check = new BankCheck(amount);

                                        BankBox box = e.Mobile.BankBox;

                                        if (!box.TryDropItem(e.Mobile, check, false))
                                        {
                                            e.Mobile.Say(500386); // There's not enough room in your bankbox for the check!
                                            check.Delete();
                                        }
                                        else if (!box.ConsumeTotal(typeof(Gold), amount))
                                        {
                                            e.Mobile.Say(500384); // Ah, art thou trying to fool me? Thou hast not so much gold!
                                            check.Delete();
                                        }
                                        else
                                        {
                                            e.Mobile.Say(1042673, amount.ToString()); // Into your bank box I have placed a check in the amount of:
                                        }
                                    }
                                }

                                break;
                            }
                    }
                }
            }
        }
    }
}
