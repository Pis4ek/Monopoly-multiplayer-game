using Playmode;
using Playmode.PlayData;
using Playmode.ServerEnteties;
using System;

namespace Mirror
{
    public static class NetworkWriterReaderExtentions
    {
        #region InputPermissions
        public static void WriteInputPermissions(this NetworkWriter writer, InputPermissions inputPermissions)
        {
            writer.WriteInt(inputPermissions.Permissions.Count);
            foreach (var perms in inputPermissions.Permissions)
            {
                writer.WriteUInt(((uint)perms.Key));
                writer.WriteBool(perms.Value);
            }
        }

        public static InputPermissions ReadInputPermissions(this NetworkReader reader)
        {
            int size = reader.ReadInt();
            InputPermissions newInput = new InputPermissions();
            for (int i = 0; i < size; i++)
            {
                InputType inputType = (InputType)Enum.GetValues(typeof(InputType)).GetValue(reader.ReadUInt());
                if (reader.ReadBool())
                    newInput.Activate(inputType);
            }

            return newInput;
        }
        #endregion

        #region PlayerID
        public static void WritePlayerID(this NetworkWriter writer, PlayerID id)
        {
            writer.WriteUInt((uint)id);
        }

        public static PlayerID ReadPlayerID(this NetworkReader reader)
        {
            var id = (PlayerID)reader.ReadUInt();
            return id;
        }
        #endregion

        #region TradeOfferInfo
        public static void WriteTradeOfferInfo(this NetworkWriter writer, TradeOfferInfo info)
        {
            writer.WriteUInt((uint)info.Proposer);
            writer.WriteUInt((uint)info.Reciever);
            writer.WriteUInt((uint)info.Payer);
            writer.WriteInt(info.Surcharge);

            writer.WriteInt(info.CellsToProposer.Count);
            foreach (var index in info.CellsToProposer)
            {
                writer.WriteInt(index);
            }

            writer.WriteInt(info.CellsToReciever.Count);
            foreach (var index in info.CellsToReciever)
            {
                writer.WriteInt(index);
            }
        }

        public static TradeOfferInfo ReadTradeOfferInfo(this NetworkReader reader)
        {
            var info = new TradeOfferInfo();

            info.Proposer = (PlayerID)reader.ReadUInt();
            info.Reciever = (PlayerID)reader.ReadUInt();
            info.Payer = (PlayerID)reader.ReadUInt();
            info.Surcharge = reader.ReadInt();

            var size = reader.ReadInt();
            for (int i = 0; i < size; i++)
            {
                info.CellsToProposer.Add(reader.ReadInt());
            }

            size = reader.ReadInt();
            for (int i = 0; i < size; i++)
            {
                info.CellsToReciever.Add(reader.ReadInt());
            }

            return info;
        }
        #endregion

        #region ThrowCubesResult
        public static void WriteThrowCubesResult(this NetworkWriter writer, ThrowCubesResult result)
        {
            writer.WriteUInt((uint)result.Thrower);
            writer.WriteInt(result.Cube1Result);
            writer.WriteInt(result.Cube2Result);
        }

        public static ThrowCubesResult ReadThrowCubesResult(this NetworkReader reader)
        {
            var id = (PlayerID)reader.ReadUInt();
            var value1 = reader.ReadInt();
            var value2 = reader.ReadInt();
            return new ThrowCubesResult(id, value1, value2);
        }
        #endregion

        #region DateTime
        public static void WriteDateTime(this NetworkWriter writer, DateTime time)
        {
            writer.WriteInt(time.Year);
            writer.WriteInt(time.Month);
            writer.WriteInt(time.Day);
            writer.WriteInt(time.Hour);
            writer.WriteInt(time.Minute);
            writer.WriteInt(time.Second);
            writer.WriteInt(time.Millisecond);
        }

        public static DateTime ReadDateTime(this NetworkReader reader)
        {
            var year = reader.ReadInt();
            var month = reader.ReadInt();
            var day = reader.ReadInt();
            var hour = reader.ReadInt();
            var minute = reader.ReadInt();
            var second = reader.ReadInt();
            var millisecond = reader.ReadInt();
            return new DateTime(year, month, day, hour, minute, second, millisecond);
        }
        #endregion

        #region Log
        public static void WriteLog(this NetworkWriter writer, Log log)
        {
            writer.WriteUInt((uint)log.Author);
            writer.WriteString(log.Text);
            writer.WriteInt(log.Index);
        }

        public static Log ReadLog(this NetworkReader reader)
        {
            var author = reader.ReadUInt();
            var text = reader.ReadString();
            var inde = reader.ReadInt();
            return new Log((PlayerID)author, text, inde);
        }
        #endregion
    }
}
