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
        public string ObjectCard { get; set; }
        /// <summary>
        /// Фото схемы объекта
        /// </summary>
        public string ObjectScheme { get; set; }
        /// <summary>
        /// Фото расшлейфовки объекта
        /// </summary>
        public string ObjectWiring { get; set; }
        /// <summary>
        /// Фото вывески
        /// </summary>
        public string ObjectSignboard { get; set; }
        /// <summary>
        /// Фото списка ответственныъ
        /// </summary>
        public string ObjectListResponsible { get; set; }
        /// <summary>
        /// Доп. фото 1
        /// </summary>
        public string ObjectExtra1 { get; set; }
        /// <summary>
        /// Доп. фото 2
        /// </summary>
        public string ObjectExtra2 { get; set; }
        /// <summary>
        /// Доп. фото 3
        /// </summary>
        public string ObjectExtra3 { get; set; }
        /// <summary>
        /// Отображает статус записи в базе данных. 0-не отправлен; 1-отправлен;-1-удален пользователем;
        /// </summary>
        public int State { get; set; }
        //Отображает инфу о монтаже, если такой был в гугл таблице
        public string GoogleComment { get; set; }
        /// <summary>
        /// Отражает дату и время отправки монтажа
        /// </summary>
        public DateTime? DateSended { get; set; }
        /// <summary>
        /// Если очень хочется, то в это поле можно записать какую угодно инфу и показывать юзверю
        /// </summary>
        public string CompositeName {
            get {
                if (DateSended.HasValue)
                    return "№: " + ObjectNumber + "(" + DateSended.Value.ToString() + ")";
                else
                    return "№: " + ObjectNumber;
            }
            //get => "№: " + ObjectNumber + "(" +DateSended.Value.ToString()+ ")";
        }
    }
}
