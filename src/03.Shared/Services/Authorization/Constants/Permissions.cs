namespace Zeta.NontonFilm.Shared.Services.Authorization.Constants;

public static class Permissions
{
    #region Essential Permissions
    public const string NontonFilm_Audit_Index = "nontonfilm.audit.index";
    public const string NontonFilm_Audit_View = "nontonfilm.audit.view";
    public const string NontonFilm_HealthCheck_View = "nontonfilm.healthcheck.view";
    #endregion Essential Permissions

    #region Business Permissions
    public const string NontonFilm_Movie_View = "nontonfilm.movie.view";
    public const string NontonFilm_Movie_Index = "nontonfilm.movie.index";
    public const string NontonFilm_Movie_Edit = "nontonfilm.movie.edit";
    public const string NontonFilm_Movie_Delete = "nontonfilm.movie.delete";
    public const string NontonFilm_Movie_Add = "nontonfilm.movie.add";
    public const string NontonFilm_Movie_NowShowing = "nontonfilm.movie.nowshowing";

    public const string NontonFilm_Genre_View = "nontonfilm.genre.view";
    public const string NontonFilm_Genre_Index = "nontonfilm.genre.index";
    public const string NontonFilm_Genre_Edit = "nontonfilm.genre.edit";
    public const string NontonFilm_Genre_Delete = "nontonfilm.genre.delete";
    public const string NontonFilm_Genre_Add = "nontonfilm.genre.add";

    public const string NontonFilm_Cinema_View = "nontonfilm.cinema.view";
    public const string NontonFilm_Cinema_Index = "nontonfilm.cinema.index";
    public const string NontonFilm_Cinema_Edit = "nontonfilm.cinema.edit";
    public const string NontonFilm_Cinema_Delete = "nontonfilm.cinema.delete";
    public const string NontonFilm_Cinema_Add = "nontonfilm.cinema.add";
    public const string NontonFilm_Cinema_User_Handle = "nontonfilm.cinema.user.handle";

    public const string NontonFilm_CinemaChain_View = "nontonfilm.cinemachain.view";
    public const string NontonFilm_CinemaChain_Index = "nontonfilm.cinemachain.index";
    public const string NontonFilm_CinemaChain_Edit = "nontonfilm.cinemachain.edit";
    public const string NontonFilm_CinemaChain_Delete = "nontonfilm.cinemachain.delete";
    public const string NontonFilm_CinemaChain_Add = "nontonfilm.cinemachain.add";

    public const string NontonFilm_City_View = "nontonfilm.city.view";
    public const string NontonFilm_City_Index = "nontonfilm.city.index";
    public const string NontonFilm_City_Edit = "nontonfilm.city.edit";
    public const string NontonFilm_City_Delete = "nontonfilm.city.delete";
    public const string NontonFilm_City_Add = "nontonfilm.city.add";

    public const string NontonFilm_Show_View = "nontonfilm.show.view";
    public const string NontonFilm_Show_Index_Upcoming = "nontonfilm.show.index.upcoming";
    public const string NontonFilm_Show_Index_Past = "nontonfilm.show.index.past";
    public const string NontonFilm_Show_Edit = "nontonfilm.show.edit";
    public const string NontonFilm_Show_Delete = "nontonfilm.show.delete";
    public const string NontonFilm_Show_Add = "nontonfilm.show.add";

    public const string NontonFilm_Studio_View = "nontonfilm.studio.view";
    public const string NontonFilm_Studio_Index = "nontonfilm.studio.index";
    public const string NontonFilm_Studio_Edit = "nontonfilm.studio.edit";
    public const string NontonFilm_Studio_Delete = "nontonfilm.studio.delete";
    public const string NontonFilm_Studio_Add = "nontonfilm.studio.add";

    public const string NontonFilm_Ticket_User_Handle = "nontonfilm.ticket.user.handle";
    #endregion Business Permissions

    public static readonly string[] All = new string[]
    {
        #region Essential Permissions
        NontonFilm_Audit_Index,
        NontonFilm_Audit_View,
        NontonFilm_HealthCheck_View,
        #endregion Essential Permissions

        #region Business Permissions
        NontonFilm_Movie_View,
        NontonFilm_Movie_Index,
        NontonFilm_Movie_Delete,
        NontonFilm_Movie_Edit,
        NontonFilm_Movie_Add,
        NontonFilm_Movie_NowShowing,

        NontonFilm_Genre_View,
        NontonFilm_Genre_Index,
        NontonFilm_Genre_Delete,
        NontonFilm_Genre_Edit,
        NontonFilm_Genre_Add,

        NontonFilm_Cinema_View,
        NontonFilm_Cinema_Index,
        NontonFilm_Cinema_Delete,
        NontonFilm_Cinema_Edit,
        NontonFilm_Cinema_Add,
        NontonFilm_Cinema_User_Handle,

        NontonFilm_CinemaChain_View,
        NontonFilm_CinemaChain_Index,
        NontonFilm_CinemaChain_Delete,
        NontonFilm_CinemaChain_Edit,
        NontonFilm_CinemaChain_Add,

        NontonFilm_City_View,
        NontonFilm_City_Index,
        NontonFilm_City_Delete,
        NontonFilm_City_Edit,
        NontonFilm_City_Add,

        NontonFilm_Show_View,
        NontonFilm_Show_Index_Past,
        NontonFilm_Show_Index_Upcoming,
        NontonFilm_Show_Delete,
        NontonFilm_Show_Edit,
        NontonFilm_Show_Add,

        NontonFilm_Studio_View,
        NontonFilm_Studio_Index,
        NontonFilm_Studio_Delete,
        NontonFilm_Studio_Edit,
        NontonFilm_Studio_Add,

        NontonFilm_Ticket_User_Handle,
        #endregion Business Permissions
    };
}
