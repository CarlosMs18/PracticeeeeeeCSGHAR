namespace PracticeDTORest.DTOs
{
    public class ColeccionDeRecursos<T> : Recurso where T : Recurso //ColeccionDeRecursos : clase hereda de recurso y que una 
                                                                    //coleecion de ColeccionDeRecursos que tengamos como una colecion de autoreseste autor tendra que implementar
                                                                    //la clase recurso o herdedar de ella,si nos fijamos  autodDTO hereda de recurso asi que no hay problema
    {

        public List<T> Valores { get; set; }
    }
}
