package de.deroq.virtualpet;

import com.mongodb.client.MongoClient;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.model.Filters;
import com.mongodb.client.result.DeleteResult;
import com.mongodb.client.result.InsertOneResult;
import com.mongodb.client.result.UpdateResult;
import de.deroq.virtualpet.model.MongoResponse;
import io.quarkus.cache.CacheInvalidateAll;
import io.quarkus.cache.CacheResult;
import jakarta.enterprise.context.ApplicationScoped;
import jakarta.inject.Inject;
import org.bson.BsonValue;
import org.bson.conversions.Bson;
import org.bson.types.ObjectId;
import org.eclipse.microprofile.config.ConfigProvider;
import org.jboss.logging.Logger;

import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;
import java.util.stream.StreamSupport;

/**
 * @author Miles
 * @since 16.03.2024
 */
@ApplicationScoped
public class VirtualPetService {

    @Inject
    MongoClient client;

    private static final Logger LOGGER = Logger.getLogger(VirtualPetService.class);

    private static final String DATABASE_NAME = ConfigProvider.getConfig()
            .getConfigValue("database")
            .getValue();
    private static final String COLLECTION_NAME = ConfigProvider.getConfig()
            .getConfigValue("collection")
            .getValue();

    @CacheInvalidateAll(cacheName = "pets-list-cache")
    @CacheInvalidateAll(cacheName = "pets-aliveList-cache")
    public MongoResponse createPet(VirtualPet pet) {
        VirtualPet found = this.getCollection().find(Filters.eq("name", pet.getName())).first();
        if (found != null) {
            return MongoResponse.create(302, "VirtualPet with name '" + pet.getName() + "' already exists");
        }

        InsertOneResult res = this.getCollection().insertOne(pet);
        if (!res.wasAcknowledged()) {
            return MongoResponse.create(500, "Insert was not acknowledged, maybe check logs");
        }

        BsonValue id = res.getInsertedId();
        if (id == null) {
            return MongoResponse.create(500, "Inserted Id is null, maybe check logs");
        }

        pet.setId(id.asObjectId().getValue());
        return MongoResponse.create(200, pet);
    }

    @CacheInvalidateAll(cacheName = "pet-byId-cache")
    @CacheInvalidateAll(cacheName = "pet-byName-cache")
    @CacheInvalidateAll(cacheName = "pets-list-cache")
    @CacheInvalidateAll(cacheName = "pets-aliveList-cache")
    public MongoResponse updatePet(VirtualPet pet) {
        Bson filter = null;
        if (pet.getId() != null) {
            filter = Filters.eq("_id", pet.getId());
        } else if (pet.getName() != null) {
            filter = Filters.eq("name", pet.getName());
        }

        if (filter == null) {
            return MongoResponse.create(400, "Id and name of VirtualPet is null");
        }

        UpdateResult res = this.getCollection().replaceOne(filter, pet);
        if (res.getMatchedCount() != 1) {
            LOGGER.debug("updatePet getMatchedCount: " + res.getMatchedCount());
            return MongoResponse.create(404, "VirtualPet not found");
        }

        if (!res.wasAcknowledged()) {
            return MongoResponse.create(500, "VirtualPet was not inserted, maybe check logs");
        }

        if (res.getModifiedCount() != 1) {
            LOGGER.debug("updatePet getModifiedCount: " + res.getModifiedCount());
            return MongoResponse.create(500, "VirtualPet was not modified, maybe check logs");
        }

        return MongoResponse.create(200, pet);
    }

    @CacheInvalidateAll(cacheName = "pet-byId-cache")
    @CacheInvalidateAll(cacheName = "pet-byName-cache")
    @CacheInvalidateAll(cacheName = "pets-list-cache")
    @CacheInvalidateAll(cacheName = "pets-aliveList-cache")
    public MongoResponse deletePet(Optional<String> id, Optional<String> name) {
        Bson filter = null;
        if (id.isPresent()) {
            try {
                filter = Filters.eq("_id", new ObjectId(id.get()));
            } catch (Exception e) {
                LOGGER.warn("Invalid ObjectId '" + id.get() + "'");
            }
        } else if (name.isPresent()) {
            filter = Filters.eq("name", name.get());
        }

        if (filter == null) {
            return MongoResponse.create(400, "Id and name in request is null");
        }

        DeleteResult res = this.getCollection().deleteOne(filter);
        if (!res.wasAcknowledged()) {
            return MongoResponse.create(500, "VirtualPet was not deleted, maybe check logs");
        }

        if (res.getDeletedCount() == 0) {
            return MongoResponse.create(404, "VirtualPet not found");
        }

        LOGGER.debug("deletePet getDeletedCount: " + res.getDeletedCount());
        return MongoResponse.create(200, null);
    }

    @CacheResult(cacheName = "pet-byId-cache")
    public VirtualPet getPetById(String id) {
        ObjectId objectId;
        try {
            objectId = new ObjectId(id);
        } catch (Exception e) {
            LOGGER.warn("Invalid ObjectId '" + id + "'");
            return null;
        }

        return this.getCollection().find(Filters.eq("_id", objectId)).first();
    }

    @CacheResult(cacheName = "pet-byName-cache")
    public VirtualPet getPetByName(String name) {
        return this.getCollection().find(Filters.eq("name", name)).first();
    }

    @CacheResult(cacheName = "pets-list-cache")
    public List<VirtualPet> getPets(Optional<Boolean> alive) {
        Bson filter = Filters.empty();
        if (alive.isPresent()
                && alive.get()) {
            filter = Filters.eq("alive", true);
        }
        return StreamSupport.stream(this.getCollection().find(filter).spliterator(), false)
                .collect(Collectors.toList());
    }

    @CacheResult(cacheName = "pets-aliveList-cache")
    public List<VirtualPet> getAlivePets() {
        return StreamSupport.stream(this.getCollection().find(Filters.eq("alive", true)).spliterator(), false)
                .collect(Collectors.toList());
    }

    private MongoCollection<VirtualPet> getCollection() {
        return client.getDatabase(DATABASE_NAME)
                .getCollection(COLLECTION_NAME, VirtualPet.class);
    }
}
