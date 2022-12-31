import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import userService from "../../services/userService";
import PasswordStrengthMeter from "./PasswordStrengthMeter";
import migratelyLogo from "../../assets/images/migrately/migrately-logo.png";
import "./userstyles.css";
import {
  Card,
  Col,
  Row,
  Image,
  Button,
  OverlayTrigger,
  Tooltip,
  Modal,
} from "react-bootstrap";
import { Formik, Form, Field, ErrorMessage } from "formik";
import * as validation from "../../schemas/userSchema";
import swal from "sweetalert2";
import SiteReference from "components/sitereference/SiteReference";

const Register = () => {
  const [userRegisterData] = useState({
    firstName: "",
    lastName: "",
    mi: "",
    email: "",
    password: "",
    confirmPassword: "",
    avatarUrl: "",
    isAttorney: false,
  });

  const [showPassword, setShowPassword] = useState(true);
  const [show, setShow] = useState(false);

  const navigate = useNavigate();

  const handleSubmit = (values) => {
    userService
      .registerUser(values)
      .then(onRegisterSuccess)
      .catch(onRegisterFailure);
  };

  const onRegisterSuccess = () => {
    swal
      .fire(
        "Hooray!",
        "Registration Successful! An Email has been sent to you!",
        "success",
        {
          button: "Ok",
        }
      )
      .then(navigate("/awaitconfirm"));
  };

  const onRegisterFailure = () => {
    swal.fire("Error", "Registration Failed.", "error");
  };

  const onEyeClicked = () => {
    setShowPassword((prevState) => {
      return !prevState;
    });
  };

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);

  return (
    <React.Fragment>
      <div className="user-form-background">
        <Row className="align-items-center justify-content-center g-0 min-vh-100">
          <Col lg={7} md={5} className="py-8 py-xl-0">
            <Card>
              <Card.Body className="p-6">
                <div className="mb-4">
                  <div className="user-align-center">
                    <Link to="/">
                      <Image
                        src={migratelyLogo}
                        className="mb-4"
                        alt="Migrately Logo"
                      />
                    </Link>
                  </div>
                  <h1 className="mb-1 fw-bold">Sign up</h1>
                  <span>
                    Already have an account?{" "}
                    <Link to="/login" className="ms-1">
                      Sign in
                    </Link>
                  </span>
                </div>

                <Formik
                  enableReinitialize={true}
                  initialValues={userRegisterData}
                  onSubmit={handleSubmit}
                  validationSchema={validation.validationRegister}
                >
                  {({ values, touched, errors }) => (
                    <Form>
                      <Row>
                        <Col lg={6} md={12} className="mb-3">
                          <ErrorMessage
                            name="firstName"
                            className="user-form-error"
                            component="div"
                          />
                          <label
                            htmlFor="firstName"
                            className="user-form-label-text"
                          >
                            First Name
                          </label>
                          <Field
                            type="text"
                            className="form-control"
                            name="firstName"
                          />
                        </Col>
                        <Col lg={6} md={12} className="mb-3">
                          <ErrorMessage
                            name="mi"
                            className="user-form-error"
                            component="div"
                          />
                          <label htmlFor="mi" className="user-form-label-text">
                            Middle Initial
                          </label>
                          <Field
                            type="text"
                            className="form-control"
                            name="mi"
                          />
                        </Col>

                        <Col lg={6} md={12} className="mb-3">
                          <ErrorMessage
                            name="lastName"
                            className="user-form-error"
                            component="div"
                          />
                          <label
                            htmlFor="firstName"
                            className="user-form-label-text"
                          >
                            Last Name
                          </label>
                          <Field
                            type="text"
                            className="form-control"
                            name="lastName"
                          />
                        </Col>
                        <Col lg={6} md={12} className="mb-3">
                          <ErrorMessage
                            name="email"
                            className="user-form-error"
                            component="div"
                          />
                          <label
                            htmlFor="email"
                            className="user-form-label-text"
                          >
                            Email
                          </label>
                          <Field
                            type="email"
                            className="form-control"
                            name="email"
                          />
                        </Col>
                        <Col lg={6} md={12} className="mb-3">
                          <ErrorMessage
                            name="password"
                            className="user-form-error"
                            component="div"
                          />
                          <label
                            htmlFor="password"
                            className="user-form-label-text"
                          >
                            Password
                          </label>
                          <Field
                            type={showPassword ? "password" : "text"}
                            className="form-control"
                            name="password"
                          />
                          {showPassword ? (
                            <span
                              className="fa fa-eye user-eye-toggle-password"
                              onClick={onEyeClicked}
                            />
                          ) : (
                            <span
                              className="fa fa-eye-slash user-eye-toggle-password"
                              onClick={onEyeClicked}
                            />
                          )}
                        </Col>

                        <Col lg={6} md={12} className="mb-3">
                          <ErrorMessage
                            name="confirmPassword"
                            className="user-form-error"
                            component="div"
                          />
                          <label
                            htmlFor="confirmPassword"
                            className="user-form-label-text"
                          >
                            Confirm Password
                          </label>
                          <Field
                            type={showPassword ? "password" : "text"}
                            className="form-control"
                            name="confirmPassword"
                          />
                        </Col>
                        {(touched.password || values.password !== "") && (
                          <Col lg={6} md={12} className="offset-6">
                            <div>
                              <span>
                                Password Strength
                                <OverlayTrigger
                                  placement="bottom"
                                  overlay={
                                    <Tooltip id="password-Tooltip">
                                      <p>Password should have at least</p>
                                      <ul>
                                        <li>One number: 0-9</li>
                                        <li>One Upper-case: A-Z</li>
                                        <li>One Lower-case: a-z</li>
                                        <li>A symbol: !@#$%^&*</li>
                                        <li>At least 8 characters</li>
                                      </ul>
                                    </Tooltip>
                                  }
                                >
                                  {({ ref, ...triggerHandler }) => (
                                    <i
                                      ref={ref}
                                      {...triggerHandler}
                                      id="password-Tooltip"
                                      className="fas fa-question-circle ms-1"
                                    ></i>
                                  )}
                                </OverlayTrigger>
                              </span>
                              <PasswordStrengthMeter
                                password={values.password}
                              />
                            </div>
                          </Col>
                        )}
                        <div className="mb-3">
                          <ErrorMessage
                            name="avatarUrl"
                            className="user-form-error"
                            component="div"
                          />
                          <label
                            htmlFor="avatarUrl"
                            className="user-form-label-text"
                          >
                            An Image url
                          </label>
                          <Field
                            type="text"
                            className="form-control"
                            name="avatarUrl"
                          />
                        </div>
                        <Col lg={6} md={12} className="mb-3 d-flex">
                          <ErrorMessage
                            name="isAttorney"
                            className="user-form-error"
                            component="div"
                          />
                          <label
                            htmlFor="isAttorney"
                            className="user-form-label-text "
                          >
                            Are you an Attorney?
                          </label>
                          <div className="form-check form-switch ms-2">
                            <Field
                              type="checkbox"
                              className="form-check-input"
                              name="isAttorney"
                            />
                          </div>
                        </Col>
                        <Col lg={12} md={12} className="mb-0 d-grid gap-2">
                          <Button
                            variant="primary"
                            onClick={handleShow}
                            disabled={
                              Object.keys(errors).length > 0 ||
                              Object.keys(touched).length === 0
                            }
                          >
                            Register
                          </Button>
                        </Col>
                      </Row>
                      <Modal
                        show={show}
                        onHide={handleClose}
                        backdrop="static"
                        keyboard={false}
                      >
                        <Modal.Header closeButton>
                          <Modal.Title>One Last Question.</Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                          <SiteReference
                            handleClose={handleClose}
                            newUser={values}
                            handlesSubmit={handleSubmit}
                          />
                        </Modal.Body>
                      </Modal>
                    </Form>
                  )}
                </Formik>

                <hr className="my-4" />
                <div className="mt-4 text-center">
                  {/* Facebook */}
                  <Link
                    to="#"
                    className="btn-social btn-social-outline btn-facebook"
                  >
                    <i className="fab fa-facebook"></i>
                  </Link>{" "}
                  {/* Twitter */}
                  <Link
                    to="#"
                    className="btn-social btn-social-outline btn-twitter"
                  >
                    <i className="fab fa-twitter"></i>
                  </Link>{" "}
                  {/* LinkedIn */}
                  <Link
                    to="#"
                    className="btn-social btn-social-outline btn-linkedin"
                  >
                    <i className="fab fa-linkedin"></i>
                  </Link>{" "}
                  {/* GitHub */}
                  <Link
                    to="#"
                    className="btn-social btn-social-outline btn-github"
                  >
                    <i className="fab fa-github"></i>
                  </Link>
                </div>
              </Card.Body>
            </Card>
          </Col>
        </Row>
      </div>
    </React.Fragment>
  );
};

export default Register;
