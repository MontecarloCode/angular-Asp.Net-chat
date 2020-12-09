import { TestBed } from '@angular/core/testing';
import { ApiError, LoginCredentials, User } from '@chat-client/core/models';
import { MockStore, provideMockStore } from '@ngrx/store/testing';
import { cold } from 'jasmine-marbles';
import { AuthFacade } from './facade';
import * as authSelectors from './selectors';
import * as authActions from './actions';
import { PartialState } from './state';
import { skip } from 'rxjs/operators';

describe('AuthFacade', () => {
  let facade: AuthFacade;
  let store: MockStore<PartialState>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [],
      providers: [
        AuthFacade,
        provideMockStore({ initialState: { token: 'test' }})
      ]
    });

    store = TestBed.inject(MockStore);
    facade = TestBed.inject(AuthFacade);
  });

  describe('#token$', () => {
    it('should return the states access token', () => {
      // Arrange
      const expectedToken = 'some.access.token';
      store.overrideSelector(authSelectors.selectToken, expectedToken);

      const expected = cold('(a)', { a: expectedToken });

      // Act
      const token$ = facade.token$;

      // Assert
      expect(token$).toBeObservable(expected);
    });
  });

  describe('#user$', () => {
    it('should return the states user object', () => {
      // Arrange
      const expectedUser: User = {
        userId: 1,
        userName: 'alfred_miller'
      };

      store.overrideSelector(authSelectors.selectUser, expectedUser);

      const expected = cold('(a)', { a: expectedUser });

      // Act
      const user$ = facade.user$;

      // Assert
      expect(user$).toBeObservable(expected);
    });
  });

  describe('#emailExists$', () => {
    it('should return the states indicator, whether a given email address already exists', () => {
      // Arrange
      const expectedResult = true;
      store.overrideSelector(authSelectors.selectEmailExists, expectedResult);

      const expected = cold('(a)', { a: expectedResult });

      // Act
      const emailExists$ = facade.emailExists$;

      // Assert
      expect(emailExists$).toBeObservable(expected);
    });
  });

  describe('#userNameExists$', () => {
    it('should return the states indicator, whether a given user name already exists', () => {
      // Arrange
      const expectedResult = true;
      store.overrideSelector(authSelectors.selectUserNameExists, expectedResult);

      const expected = cold('(a)', { a: expectedResult });

      // Act
      const userNameExists$ = facade.userNameExists$;

      // Assert
      expect(userNameExists$).toBeObservable(expected);
    });
  });

  describe('#isLoading$', () => {
    it('should return the states indicator, whether a request is still ongoing/loading', () => {
      // Arrange
      const expectedResult = true;
      store.overrideSelector(authSelectors.selectLoading, expectedResult);

      const expected = cold('(a)', { a: expectedResult });

      // Act
      const isLoading$ = facade.isLoading$;

      // Assert
      expect(isLoading$).toBeObservable(expected);
    });
  });

  describe('#error$', () => {
    it('should return the states current error instance', () => {
      // Arrange
      const expectedError: ApiError = {
        statusCode: 500,
        message: 'some message'
      };

      store.overrideSelector(authSelectors.selectError, expectedError);

      const expected = cold('(a)', { a: expectedError });

      // Act
      const error$ = facade.error$;

      // Assert
      expect(error$).toBeObservable(expected);
    });
  });
});
