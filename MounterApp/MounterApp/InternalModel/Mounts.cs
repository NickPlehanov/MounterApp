using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MounterApp.InternalModel {
    public class Mounts {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        /// <summary>
        /// Идентификатор монтажника
        /// </summary>
        public Guid MounterID{ get; set; }
        /// <summary>
        /// Номер объекта
        /// </summary>
        public string ObjectNumber{ get; set; }
        /// <summary>
        /// Название объекта
        /// </summary>
        public string ObjectName{ get; set; }
        /// <summary>
        /// Адрес объекта
        /// </summary>
        public string AddressName{ get; set; }
        /// <summary>
        /// Куратор по договору
        /// </summary>
        public string Curator{ get; set; }
        /// <summary>
        /// Руководитель монтажа
        /// </summary>
        public string HeadMounter{ get; set; }
        /// <summary>
        /// Поъездные пути
        /// </summary>
        public string Driveways{ get; set; }
        /// <summary>
        /// Фото карточки объекта
        /// </summary>
        public byte[] ObjectCard { get; set; }
        /// <summary>
        /// Фото схемы объекта
        /// </summary>
        public byte[] ObjectScheme { get; set; }
        /// <summary>
        /// Фото расшлейфовки объекта
        /// </summary>
        public byte[] ObjectWiring { get; set; }
        /// <summary>
        /// Фото списка ответственных
        /// </summary>
        public byte[] ObjectSignboard { get; set; }
        /// <summary>
        /// Доп. фото 1
        /// </summary>
        public byte[] ObjectExtra1 { get; set; }
        /// <summary>
        /// Доп. фото 2
        /// </summary>
        public byte[] ObjectExtra2 { get; set; }
        /// <summary>
        /// Доп. фото 3
        /// </summary>
        public byte[] ObjectExtra3 { get; set; }
        /// <summary>
        /// Отображает статус записи в базе данных. 0-не отправлен; 1-отправлен;-1-удален пользователем;
        /// </summary>
        public int State { get; set; }
    }
}
