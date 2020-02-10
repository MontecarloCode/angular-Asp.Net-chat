import { createReducer, on } from '@ngrx/store';
import * as latestMessagesActions from './actions';
import { initialState, latestMessagesAdapter, State } from './state';

const reducer = createReducer(
  initialState,
  on(latestMessagesActions.loadLatestMessages, (state) => {
    return {
      ...state,
      isLoading: true,
      error: null,
    };
  }),
  on(latestMessagesActions.loadLatestMessagesSuccess, (state, { latestMessages }) => {
    return latestMessagesAdapter.addAll(latestMessages, {
      ...state,
      isLoading: false,
      error: null,
    });
  }),
  on(latestMessagesActions.loadLatestMessagesFailure, (state, { error }) => {
    return {
      ...state,
      isLoading: false,
      error,
    };
  })
);

export function latestMessagesReducer(state: State | undefined, action: latestMessagesActions.LatestMessagesUnion): State {
  return reducer(state, action);
}
