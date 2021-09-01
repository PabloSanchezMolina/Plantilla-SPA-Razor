var domSpa = document.getElementById("spa");

document.addEventListener("DOMContentLoaded", function () {
    fInicializarTrasActualizarDom();
});

function fInicializarTrasActualizarDom() {
    //se ejecuta en la primera carga y tras cada actualización del dom
    //aquí pondremos todos los eventos de usuario que debamos añadir a elementos del dom que no pudimos definir en el propio html de la vista razor
}

function fPost(sRuta, oParametros, fCallback) {
    try {
        //TODO: mostrar modal de cargando (spinner)
        fetch(sRuta, {
            method: 'POST',
            headers: {
                "Content-type": "application/json",
                "Accept": "application/json",
                "XSRF-TOKEN": document.querySelector('input[name="XSRF-TOKEN"]').value
            },
            body: JSON.stringify(oParametros)
        }).then(function json(oRespuesta) {
            oRespuesta.json().then(jRespuesta => {
                if (jRespuesta.bCorrecto) {
                    if (domSpa !== null && domSpa !== undefined) {
                        //Pisamos con el html generado por backend via razor
                        domSpa.innerHTML = jRespuesta.sHtml;
                        fInicializarTrasActualizarDom();
                    }
                    if (fCallback !== null && fCallback !== undefined) {
                        fCallback(jRespuesta);
                    }
                    //TODO: ocultar modal de cargando (spinner)
                } else {
                    //TODO: mostrar mensaje de error al usuario recibido en jRespuesta.sMotivo
                    //TODO: ocultar modal de cargando (spinner)
                }
            });
        }).catch(function (error) {
            //TODO: mostrar mensaje de error al usuario
            //TODO: ocultar modal de cargando (spinner)
        });
    } catch (error) {
        //TODO: mostrar mensaje de error al usuario
        //TODO: ocultar modal de cargando (spinner)
    }
}

//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@//

function fGuardarNombreDeUsuario(dom) {
    fPost("/api/guardar-nombre-de-usuario", { sUsuario: dom.value.trim() });
}

function fCrearLista() {
    var dom = document.getElementById("txtNombreNuevaLista");
    if (dom !== null && dom !== undefined) {
        var sLista = dom.value.trim();
        if (sLista.length > 0) {
            fPost("/api/crear-lista", { sLista: sLista });
        } else {
            //TODO: mostrar mensaje de información al usuario
        }
    }
}

function fEliminarLista(iId) {
    fPost("/api/eliminar-lista", { iId: iId });
}

function fVolverAInicio() {
    fPost("/api/volver-a-inicio", { });
}

function fEditarLista(iId) {
    fPost("/api/editar-lista", { iId: iId });
}

function fNuevoElemento() {
    var dom = document.getElementById("txtNombreNuevoElemento");
    if (dom !== null && dom !== undefined) {
        var sElemento = dom.value.trim();
        if (sElemento.length > 0) {
            fPost("/api/nuevo-elemento", { sElemento: sElemento });
        } else {
            //TODO: mostrar mensaje de información al usuario
        }
    }
}

function fEliminarElemento(iElemento) {
    fPost("/api/eliminar-elemento", { iElemento: iElemento });
}
