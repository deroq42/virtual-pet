package de.deroq.virtualpet;

import de.deroq.virtualpet.model.MongoResponse;
import jakarta.inject.Inject;
import jakarta.ws.rs.*;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;
import org.jboss.logging.Logger;

import java.util.Optional;

/**
 * @author Miles
 * @since 16.03.2024
 */
@Path("/virtualpet")
public class VirtualPetResource {

    private static final Logger LOGGER = Logger.getLogger(VirtualPetResource.class);

    @Inject
    VirtualPetService service;

    @POST
    @Consumes(MediaType.APPLICATION_JSON)
    @Produces({MediaType.APPLICATION_JSON, MediaType.TEXT_PLAIN})
    public Response createPet(VirtualPet pet) {
        try {
            if (pet == null) {
                return Response.status(Response.Status.BAD_REQUEST)
                        .entity("VirtualPet in body is null")
                        .build();
            }

            MongoResponse response = service.createPet(pet);
            return Response.status(response.getCode())
                    .entity(response.getData())
                    .build();
        } catch (Exception e) {
            LOGGER.error("Could not create pet", e);
            return Response.status(Response.Status.INTERNAL_SERVER_ERROR)
                    .entity(e.getMessage())
                    .build();
        }
    }

    @PUT
    @Consumes(MediaType.APPLICATION_JSON)
    @Produces(MediaType.TEXT_PLAIN)
    public Response updatePet(VirtualPet pet) {
        try {
            if (pet == null) {
                return Response.status(Response.Status.BAD_REQUEST)
                        .entity("VirtualPet in body is null")
                        .build();
            }

            MongoResponse response = service.updatePet(pet);
            return Response.status(response.getCode())
                    .entity(response.getData())
                    .build();
        } catch (Exception e) {
            LOGGER.error("Could not update pet", e);
            return Response.status(Response.Status.INTERNAL_SERVER_ERROR)
                    .entity(e.getMessage())
                    .build();
        }
    }

    @DELETE
    @Consumes(MediaType.TEXT_PLAIN)
    @Produces(MediaType.TEXT_PLAIN)
    public Response deletePet(@QueryParam("id") Optional<String> id, @QueryParam("name") Optional<String> name) {
        try {
            if (id.isEmpty()
                    && name.isEmpty()) {
                return Response.status(Response.Status.BAD_REQUEST)
                        .entity("id and name in query are null, but one must not be null")
                        .build();
            }

            MongoResponse response = service.deletePet(id, name);
            return Response.status(response.getCode())
                    .entity(response.getData())
                    .build();
        } catch (Exception e) {
            LOGGER.error("Could not delete pet", e);
            return Response.status(Response.Status.INTERNAL_SERVER_ERROR)
                    .entity(e.getMessage())
                    .build();
        }
    }

    @GET
    @Consumes(MediaType.TEXT_PLAIN)
    @Produces({MediaType.APPLICATION_JSON, MediaType.TEXT_PLAIN})
    public Response getPet(@QueryParam("id") Optional<String> id, @QueryParam("name") Optional<String> name) {
        try {
            if (id.isEmpty()
                    && name.isEmpty()) {
                return Response.status(Response.Status.BAD_REQUEST)
                        .entity("id and name in query are null, but one must not be null")
                        .build();
            }

            VirtualPet pet;
            if (id.isPresent()) {
                pet = service.getPetById(id.get());
            } else {
                pet = service.getPetByName(name.get());
            }
            if (pet == null) {
                return Response.status(Response.Status.NOT_FOUND)
                        .entity("VirtualPet not found")
                        .build();
            }

            return Response.status(Response.Status.OK)
                    .entity(pet)
                    .build();
        } catch (Exception e) {
            LOGGER.error("Could not get pet", e);
            return Response.status(Response.Status.INTERNAL_SERVER_ERROR)
                    .entity(e.getMessage())
                    .build();
        }
    }

    @GET
    @Path("/list")
    @Produces(MediaType.APPLICATION_JSON)
    public Response listPets(@QueryParam("alive") Optional<Boolean> alive) {
        try {
            return Response.status(Response.Status.OK)
                    .entity(service.getPets(alive))
                    .build();
        } catch (Exception e) {
            LOGGER.error("Could not get pet", e);
            return Response.status(Response.Status.INTERNAL_SERVER_ERROR)
                    .entity(e.getMessage())
                    .build();
        }
    }
}
