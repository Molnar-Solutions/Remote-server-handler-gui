import { applyMiddleware, createStore } from "redux";
import storage from "redux-persist/lib/storage";
import {persistReducer, persistStore} from "redux-persist";
import {rootReducer} from "./root-reducer";

const persisConfig = {
    key: "root",
    storage,
    blacklist: []
}

const persistedReducer = persistReducer(persisConfig, rootReducer);

const middlewares = [
];

const enhancers = applyMiddleware(...middlewares);

export const store = createStore(
    persistedReducer,
    undefined, // default state
    enhancers
)

export const persistor = persistStore(store);